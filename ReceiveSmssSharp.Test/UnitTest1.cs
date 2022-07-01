using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using ReceiveSmssSharp;

namespace ReceiveSmssSharp.Test
{
    [TestClass]
    public class UnitTest1
    {
        ReceiveSmss sms = new ReceiveSmss();
        
        [TestMethod]
        public async Task GetNumbers()
        {
            var numbers = await sms.GetNumbersAsync();

            Assert.AreNotEqual(numbers.Count, 0);
        }

        [TestMethod]
        public async Task GetSmsMessages()
        {
            var numbers = await sms.GetNumbersAsync();

            Assert.AreNotEqual(numbers.Count, 0);

            var messages = await sms.GetSmsMessagesAsync(numbers[0]);

            Assert.AreNotEqual(messages.Count, 0);
        }
    }
}