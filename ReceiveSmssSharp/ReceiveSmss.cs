using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using ReceiveSmssSharp.Models;

namespace ReceiveSmssSharp
{
	public class ReceiveSmss
	{
		private readonly HttpClient httpClient;

		private static readonly Regex _numberRegex = new Regex("href=\"\\/sms\\/[0-9]{11}\"", RegexOptions.Compiled);

		public ReceiveSmss()
		{
			httpClient = new HttpClient();

			httpClient.BaseAddress = new Uri("https://receive-smss.com/");
		}

		/// <summary>
		/// Get all available numbers from Receive-SMSS homepage
		/// </summary>
		/// <returns>List of all available numbers</returns>
		public async Task<List<string>> GetNumbersAsync()
		{
			var ret = new List<string>();

            var resp = await httpClient.GetAsync("/");

			var matches = _numberRegex.Matches(await resp.Content.ReadAsStringAsync());

			foreach (Match match in matches)
            {
				// Add number to list
				ret.Add(match.Value.Substring(11, 11));
            }

            return ret;
		}

		/// <summary>
		/// Get last 50 messages from a Receive-SMSS number
		/// </summary>
		/// <param name="number">Receive-SMSS number</param>
		/// <returns>List of last 50 SMS messages sent to number</returns>
		public async Task<List<SmsMessage>> GetSmsMessagesAsync(string number)
        {
			var resp = await httpClient.GetAsync($"/sms/{number}");

			var doc = new HtmlDocument();
			doc.LoadHtml(await resp.Content.ReadAsStringAsync());

			List<SmsMessage> table = doc.DocumentNode.SelectSingleNode("//table")		// Get the main table tage
			.Descendants("tr")															// Get all rows
			.Skip(1)																	// Skip the header
			.Where(tr => tr.Elements("td").Count() == 3)								// Make sure all rows have 3 columns
			.Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList()) // Parse out the columns
			.Select(tr => new SmsMessage(){ From=tr[0], Date=tr[1], Content=tr[2] })	// Map into SmsMessage model
			.ToList();																	// TODO: Return IEnumerable

			return table;
        }
	}
}