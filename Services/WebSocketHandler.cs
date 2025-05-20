namespace api_for_sambapos.Services
{
    using System.Net.WebSockets;
    using System.Text;
    using System.Collections.Concurrent;
    using System.Text.Json;

    public class WebSocketHandler
    {
        private static readonly ConcurrentDictionary<string, WebSocket> _sockets = new();

        public static void AddSocket(string id, WebSocket socket)
        {
            _sockets.TryAdd(id, socket);
        }

        public static async Task SendMessageToAllAsync(string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            var segment = new ArraySegment<byte>(buffer);

            foreach (var pair in _sockets)
            {
                var socket = pair.Value;
                if (socket.State == WebSocketState.Open)
                {
                    try
                    {
                        await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    catch
                    {
                        _sockets.TryRemove(pair.Key, out _); // sorunlu socket'i kaldır
                    }
                }
            }
        }

        public static async Task HandleConnectionAsync(WebSocket socket)
        {
            var id = Guid.NewGuid().ToString();
            AddSocket(id, socket);
            var buffer = new byte[1024 * 4];

            try
            {
                while (socket.State == WebSocketState.Open)
                {
                    var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        Console.WriteLine($"🛑 Client requested close: {id}");
                        break;
                    }

                    var receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine($"📥 Received from {id}: {receivedMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ WebSocket error with {id}: {ex.Message}");
            }
            finally
            {
                if (socket.State != WebSocketState.Closed)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                }
                if (socket.State == WebSocketState.Open)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by server", CancellationToken.None);
                }
                _sockets.TryRemove(id, out _);
                Console.WriteLine($"👋 Connection closed: {id}");
            }
        }
    }
}
