using Com.BigWin.Frontend.Data;
using Newtonsoft.Json;
using System;

namespace Assets.Scripts.Games.FunTarget.ApiData
{
    class LastRound
    {
        private string userId;
        private int gameId;
        private Action onRequetcomplete;

        public LastRound(string userId, int gameId)
        {
            this.userId = userId;
            this.gameId = gameId;
        }
        public void SendRequestToServer(Action OnRequestComplete)
        {
            User user = new User { game = gameId, user_id = userId };
            onRequetcomplete = OnRequestComplete;
            WebRequestHandler.instance.Post(Constant.CURRENT_ROUND_URL, JsonConvert.SerializeObject(user), OnServerResponse);
        }

        private void OnServerResponse(string arg1, bool arg2)
        {
            throw new NotImplementedException();
        }
    }
}
