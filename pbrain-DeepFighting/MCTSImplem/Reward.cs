using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbrain_DeepFighting.MCTSImplem
{
    class Reward
    {
        Dictionary<int, int> _rewards = new Dictionary<int, int>();
        public Reward(int reward1, int reward2)
        {
            _rewards.Add(1, reward1);
            _rewards.Add(2, reward2);
        }

        public Dictionary<int, int> getReward()
        {
            return _rewards;
        }

        public int GetRewardForPlayer(int playerNumber)
        {
            return _rewards[playerNumber];
        }

        public void AddReward(Reward reward)
        {
            _rewards[1] += reward.GetRewardForPlayer(1);
            _rewards[2] += reward.GetRewardForPlayer(2);
        }
    }
}
