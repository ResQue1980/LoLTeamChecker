using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LoLTeamChecker.Storage;

namespace LoLTeamChecker.Gui.Controls
{
    public partial class TeamControl : UserControl
    {
        /// <summary>
        /// This control is used for sizing and such.
        /// </summary>
        protected PlayerControl BasePlayer;

        const int PlayersStartY = 36;
        const int PlayersYSpacing = 10;

        protected int teamsize;
        public int TeamSize
        {
            get
            {
                return teamsize;
            }
            set
            {
                teamsize = value;

                foreach (PlayerControl p in Players)
                    p.Dispose();
                Players.Clear();

                for (int i = 0; i < value; i++)
                {
                    var control = new PlayerControl(this);
                    control.Location = new Point(1, PlayersStartY + (BasePlayer.Height + PlayersYSpacing) * i);
                    control.ContextMenuStrip = PlayerContextMenuStrip;
					control.Width = this.Width-2;
                    control.Visible = false;
					control.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left;
                    Players.Add(control);
                    Controls.Add(control);
                }

				Height = PlayersStartY + ((BasePlayer.Height + PlayersYSpacing) * teamsize);
            }
        }

		public PlayerControl this[int idx]
		{
			get { return Players[idx]; }
		}

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<PlayerControl> Players { get; set; }

        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true)]
        public override string Text
        {
            get
            {
                return NameLabel.Text;
            }
            set
            {
                NameLabel.Text = value;
            }
        }

        protected ContextMenuStrip playercontextmenustrip;
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true)]
        public ContextMenuStrip PlayerContextMenuStrip
        {
            get
            {
                return playercontextmenustrip;
            }
            set
            {
                playercontextmenustrip = value;
                foreach (var plr in Players)
                    plr.ContextMenuStrip = value;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            base.OnPaint(e);
            e.Graphics.DrawRectangle(Pens.Black, new Rectangle(0,0,Width-1,Height-1));
        }

        public TeamControl()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            Players = new List<PlayerControl>();
            BasePlayer = new PlayerControl();
            TeamSize = 5;
            InitializeComponent();
        }
    }
}
