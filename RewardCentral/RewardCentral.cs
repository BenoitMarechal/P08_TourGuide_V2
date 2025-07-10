using RewardCentral.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RewardCentral;

public class RewardCentral
{
    private static readonly SemaphoreSlim rewardLimiter = new(1000, 1000);

    public async Task<int> GetAttractionRewardPoints(Guid attractionId, Guid userId)
    {
        await rewardLimiter.WaitAsync();
        try
        {
            int randomDelay = Random.Shared.Next(1, 1000);
            await Task.Delay(randomDelay);

            return Random.Shared.Next(1, 1000);
        }
        finally
        {
            rewardLimiter.Release();
        }
    }
}

