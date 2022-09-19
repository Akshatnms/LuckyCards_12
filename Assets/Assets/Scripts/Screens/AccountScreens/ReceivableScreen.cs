using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountScreens
{
    public class ReceivableScreen:ScreenBlueprint
    {
        public override ScreenName ScreenID =>ScreenName.RECEIVABLES_SCREEN;

        public override void Hide()
        {
            base.Hide();
        }

        public override void Initialize(ScreenController screenController)
        {
            base.Initialize(screenController);
        }

        public override void Show(object data = null)
        {
            base.Show(data);
        }
    }
}
