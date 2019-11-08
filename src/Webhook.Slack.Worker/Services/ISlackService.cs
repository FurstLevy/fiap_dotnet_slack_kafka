using System.Threading.Tasks;

namespace Webhook.Slack.Worker.Services
{
    public interface ISlackService
    {
        Task PostSlackAsync(string payload);
    }
}
