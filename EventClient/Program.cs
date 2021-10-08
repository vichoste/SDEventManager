using System.Text;

using EzSockets;

var socket = new EzSocket(null, 5000, new EzEventsListener() {
	OnNewConnectionHandler = (EzSocket socket) => {
		Console.WriteLine($"Connection established");
	},
	OnConnectionClosedHandler = (EzSocket socket) => {
		Console.WriteLine($"Connection closed");
	},
	OnMessageReadHandler = (EzSocket sock, byte[] buff) => {
		Console.WriteLine($"{Encoding.Default.GetString(buff)}");
	},
	OnExceptionHandler = (EzSocket socket, Exception ex) => {
		Console.WriteLine($"Server not found");
		return ExceptionHandlerResponse.CloseSocket;
	}
});

socket.StartReadingMessages();

var loop = true;

do {
	var input = Console.ReadLine();
	if (input is "QUIT") {
		break;
	} else {
		socket.SendMessage(input);
	}
} while (loop);

socket.StopReadingMessages();

socket.Close();