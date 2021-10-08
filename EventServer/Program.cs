using System.Text;

using EventServer;

using EzSockets;

var people = new PersonViewModel();

Console.WriteLine($"I am the event server");

var server = new EzSocketListener(new EzEventsListener() {
	OnNewConnectionHandler = (EzSocket socket) => {
		Console.WriteLine($"I have a new connection");
		socket.SendMessage($"Enter your commands (GET - ADD [your name here])\nExample: GET\nExample: ADD Vicente Calderon\nTo disconnect, send QUIT\nPlease don't use extended ASCII characters");
		socket.StartReadingMessages();
	},
	OnConnectionClosedHandler = (EzSocket socket) => {
		Console.WriteLine($"My connection has been closed");
		socket.StopReadingMessages();
	},
	OnMessageSendHandler = (EzSocket socket, byte[] data) => Console.WriteLine($"I've sent a message to the socket"),
	OnExceptionHandler = (EzSocket socket, Exception ex) => {
		Console.WriteLine("My connection has been forcefully closed");
		return ExceptionHandlerResponse.CloseSocket;
	},
	OnMessageReadHandler = (EzSocket socket, byte[] data) => {
		var receivedData = Encoding.Default.GetString(data).Split(' ');
		if (receivedData[0] is "GET" && receivedData.Length is 1 && people.Count is > 0) {
			var stringBuilder = new StringBuilder();
			for (var i = 0; i < people.Count; i++) {
				_ = stringBuilder.Append($"{i + 1}) {people[i]?.Name}\n");
			}
			socket.SendMessage(stringBuilder.ToString());
		} else if (receivedData[0] is "GET" && receivedData.Length is 1 && people.Count is 0) {
			socket.SendMessage($"List is empty");
		} else if (receivedData[0] is "ADD" && receivedData.Length is > 1) {
			var name = string.Join(' ', receivedData, 1, receivedData.Length - 1);
			people.AddPerson(name);
			socket.SendMessage($"Added {name}");
		} else {
			socket.SendMessage($"You entered an invalid operation");
		}
	},
});

server.Listen(5000);