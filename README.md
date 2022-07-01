# ReceiveSmssSharp
A C# library for https://receive-smss.com/

## Usage

```cs
// Initialize the class
ReceiveSmss sms = new ReceiveSmss();

// Get available numbers from the home page
List<string> numbers = await sms.GetNumbersAsync();

// Get last 50 messages from a Receive-SMSS number
List<SmsMessage> messages = await sms.GetSmsMessagesAsync(numbers[0]);
```
