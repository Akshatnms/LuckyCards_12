using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountScreens
{
    public class PointTransfer : ScreenBlueprint
    {
        public override ScreenName ScreenID => ScreenName.POINT_TRANSFER_SCREEN;
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
