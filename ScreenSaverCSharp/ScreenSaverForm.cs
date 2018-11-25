using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace ScreenSaverCSharp
{
    public partial class ScreenSaverForm : Form
    {
        private System.Drawing.Point mouseLocation;

        private Device device = null;

        private VertexBuffer vb = null;

        float angle = 0.0f;

        CustomVertex.PositionColored[] verts = null;
        
        public ScreenSaverForm()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
            InitializeComponent();
            InitializeGraphics();
        }

        private void InitializeGraphics()
        {
            PresentParameters pp = new PresentParameters();
            pp.Windowed = true;
            pp.SwapEffect = SwapEffect.Discard;

            device = new Device(0, DeviceType.Hardware, this, CreateFlags.HardwareVertexProcessing, pp);

            vb = new VertexBuffer(typeof(CustomVertex.PositionColored), 1128, device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionColored.Format, Pool.Default);
            vb.Created += new EventHandler(this.OnVertexBufferCreate);
            OnVertexBufferCreate(vb, null);

            vb.SetData(verts, 0, LockFlags.None);
        }

        private void OnVertexBufferCreate(object sender, EventArgs e)
        {
            VertexBuffer buffer = (VertexBuffer)sender;
            
            float[,] circle = {
                {-0.000002f, 6.000000f, 0.000000f},
                {1.170540f, 5.884712f, 0.000000f},
                {2.296099f, 5.543278f, 0.000000f},
                {3.333420f, 4.988819f, 0.000000f},
                {4.242639f, 4.242642f, 0.000000f},
                {4.988817f, 3.333423f, 0.000000f},
                {5.543276f, 2.296103f, 0.000000f},
                {5.884711f, 1.170544f, 0.000000f},
                {6.000000f, 0.000000f, 0.000000f},
                {-6.000000f, 0.000004f, 0.000000f},
                {-5.884711f, 1.170546f, 0.000000f},
                {-5.543275f, 2.296105f, 0.000000f},
                {-4.988814f, 3.333426f, 0.000000f},
                {-4.242637f, 4.242645f, 0.000000f},
                {-3.333416f, 4.988821f, 0.000000f},
                {-2.296094f, 5.543280f, 0.000000f},
                {-1.170535f, 5.884713f, 0.000000f},
                {-0.000001f, 4.097118f, 0.000000f},
                {0.799307f, 4.018394f, 0.000000f},
                {1.567898f, 3.785243f, 0.000000f},
                {2.276236f, 3.406630f, 0.000000f},
                {2.897099f, 2.897101f, 0.000000f},
                {3.406628f, 2.276238f, 0.000000f},
                {1.999999f, 2.000001f, 0.000000f},
                {-4.097117f, 0.000000f, 0.000000f},
                {-4.018392f, 0.799312f, 0.000000f},
                {-3.785241f, 1.567903f, 0.000000f},
                {-3.406626f, 2.276240f, 0.000000f},
                {-2.897097f, 2.897103f, 0.000000f},
                {-2.276233f, 3.406632f, 0.000000f},
                {-1.567894f, 3.785245f, 0.000000f},
                {-0.799303f, 4.018394f, 0.000000f}
            };

            float[,] plane = {
                {-9.000000f, 3.000000f, 0.000000f},
                {-6.000000f, 0.000000f, 0.000000f},
                {-9.000000f, -3.000000f, 0.000000f},
                {-9.000000f, 0.000000f, 0.000000f},
                {9.000000f, 3.000000f, 0.000000f},
                {6.000000f, 0.000000f, 0.000000f},
                {9.000000f, -3.000000f, 0.000000f},
                {9.000000f, 0.000000f, 0.000000f}
            };

            float[,] plane1 = {
                {9.000000f, -9.000000f},
                {6.000000f, -6.000000f},
                {5.303294f, -5.303308f},
                {4.166769f, -6.236028f},
                {4.454370f, -6.000000f},
                {2.870116f, -6.929101f},
                {1.463166f, -7.355892f},
                {0.000000f, -9.000000f},
                {0.000000f, -7.500000f}
            };

            for (int k = 0; k < 32; k++)
            {
                circle[k, 0] = circle[k, 0] * 0.5625f;
            }

            for (int k = 0; k < 8; k++)
            {
                plane[k, 0] = plane[k, 0] * 0.5625f;
            }

            for (int k = 0; k < 9; k++)
            {
                plane1[k, 0] = plane1[k, 0] * 0.5625f;
            }

            float nol = 2.0f;
            int i = 0;

            verts = new CustomVertex.PositionColored[1128];

            //plane1 connections
            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], plane1[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], plane1[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], plane1[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], -plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], -plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], -plane1[0, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], -plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], -plane1[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], -plane1[0, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], plane1[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[2, 0], plane1[2, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[2, 0], plane1[2, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], plane1[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[2, 0], plane1[2, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[2, 0], plane1[2, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[2, 0], plane1[2, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], plane1[0, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[2, 0], plane1[2, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], plane1[0, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], -plane1[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[2, 0], -plane1[2, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[2, 0], -plane1[2, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], -plane1[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], -plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[2, 0], -plane1[2, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[2, 0], -plane1[2, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[2, 0], -plane1[2, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], -plane1[0, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[2, 0], -plane1[2, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], -plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], -plane1[0, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[2, 0], plane1[2, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[3, 0], plane1[3, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[3, 0], plane1[3, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[2, 0], plane1[2, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[2, 0], plane1[2, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[3, 0], plane1[3, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[3, 0], plane1[3, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[3, 0], plane1[3, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[2, 0], plane1[2, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[3, 0], plane1[3, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[2, 0], plane1[2, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[2, 0], plane1[2, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[2, 0], -plane1[2, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[3, 0], -plane1[3, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[3, 0], -plane1[3, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[2, 0], -plane1[2, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[2, 0], -plane1[2, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[3, 0], -plane1[3, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[3, 0], -plane1[3, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[3, 0], -plane1[3, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[2, 0], -plane1[2, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[3, 0], -plane1[3, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[2, 0], -plane1[2, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[2, 0], -plane1[2, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[3, 0], plane1[3, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[3, 0], plane1[3, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[5, 0], plane1[5, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[3, 0], plane1[3, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[5, 0], plane1[5, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[5, 0], plane1[5, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[5, 0], plane1[5, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[3, 0], plane1[3, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[3, 0], plane1[3, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[5, 0], plane1[5, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[5, 0], plane1[5, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[3, 0], plane1[3, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[3, 0], -plane1[3, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[3, 0], -plane1[3, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[5, 0], -plane1[5, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[3, 0], -plane1[3, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[5, 0], -plane1[5, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[5, 0], -plane1[5, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[5, 0], -plane1[5, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[3, 0], -plane1[3, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[3, 0], -plane1[3, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[5, 0], -plane1[5, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[5, 0], -plane1[5, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[3, 0], -plane1[3, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[5, 0], plane1[5, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[5, 0], plane1[5, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[6, 0], plane1[6, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[5, 0], plane1[5, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[6, 0], plane1[6, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[6, 0], plane1[6, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[6, 0], plane1[6, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[5, 0], plane1[5, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[5, 0], plane1[5, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[6, 0], plane1[6, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[6, 0], plane1[6, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[5, 0], plane1[5, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[5, 0], -plane1[5, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[5, 0], -plane1[5, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[6, 0], -plane1[6, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[5, 0], -plane1[5, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[6, 0], -plane1[6, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[6, 0], -plane1[6, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[6, 0], -plane1[6, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[5, 0], -plane1[5, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[5, 0], -plane1[5, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[6, 0], -plane1[6, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[6, 0], -plane1[6, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[5, 0], -plane1[5, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[6, 0], plane1[6, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[8, 0], plane1[8, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[8, 0], plane1[8, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[6, 0], plane1[6, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[6, 0], plane1[6, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[8, 0], plane1[8, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[8, 0], plane1[8, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[8, 0], plane1[8, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[6, 0], plane1[6, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[8, 0], plane1[8, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[6, 0], plane1[6, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[6, 0], plane1[6, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[6, 0], -plane1[6, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[8, 0], -plane1[8, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[8, 0], -plane1[8, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[6, 0], -plane1[6, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[6, 0], -plane1[6, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[8, 0], -plane1[8, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[8, 0], -plane1[8, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[8, 0], -plane1[8, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[6, 0], -plane1[6, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[8, 0], -plane1[8, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[6, 0], -plane1[6, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[6, 0], -plane1[6, 1], nol, Color.Blue.ToArgb()); i++;

            //plane connections
            verts[i] = new CustomVertex.PositionColored(plane[0, 0], plane[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane[0, 0], plane[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane[2, 0], plane[2, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane[0, 0], plane[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane[2, 0], plane[2, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane[2, 0], plane[2, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane[2, 0], plane[2, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane[0, 0], plane[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane[0, 0], plane[0, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane[2, 0], plane[2, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane[2, 0], plane[2, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane[0, 0], plane[0, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane[1, 0], plane[1, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane[0, 0], plane[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane[0, 0], plane[0, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane[1, 0], plane[1, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane[1, 0], plane[1, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane[0, 0], plane[0, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane[0, 0], -plane[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane[0, 0], -plane[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane[1, 0], -plane[1, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane[0, 0], -plane[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane[1, 0], -plane[1, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane[1, 0], -plane[1, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane[1, 0], -plane[1, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane[0, 0], -plane[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane[0, 0], -plane[0, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane[1, 0], -plane[1, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane[1, 0], -plane[1, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane[0, 0], -plane[0, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane[0, 0], plane[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane[0, 0], plane[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane[1, 0], plane[1, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane[0, 0], plane[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane[1, 0], plane[1, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane[1, 0], plane[1, 1], nol, Color.Blue.ToArgb()); i++;

            //circle connections
            verts[i] = new CustomVertex.PositionColored(circle[23, 0], circle[23, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[8, 0], circle[8, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[8, 0], circle[8, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[23, 0], circle[23, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[23, 0], circle[23, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[8, 0], circle[8, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[22, 0], circle[22, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[23, 0], circle[23, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[23, 0], circle[23, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[22, 0], circle[22, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[22, 0], circle[22, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[23, 0], circle[23, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[21, 0], circle[21, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[22, 0], circle[22, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[22, 0], circle[22, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[21, 0], circle[21, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[21, 0], circle[21, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[22, 0], circle[22, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[20, 0], circle[20, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[21, 0], circle[21, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[21, 0], circle[21, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[20, 0], circle[20, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[20, 0], circle[20, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[21, 0], circle[21, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[19, 0], circle[19, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[20, 0], circle[20, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[20, 0], circle[20, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[19, 0], circle[19, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[19, 0], circle[19, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[20, 0], circle[20, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[18, 0], circle[18, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[19, 0], circle[19, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[19, 0], circle[19, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[18, 0], circle[18, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[18, 0], circle[18, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[19, 0], circle[19, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[17, 0], circle[17, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[18, 0], circle[18, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[18, 0], circle[18, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[17, 0], circle[17, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[17, 0], circle[17, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[18, 0], circle[18, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[31, 0], circle[31, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[17, 0], circle[17, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[17, 0], circle[17, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[31, 0], circle[31, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[31, 0], circle[31, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[17, 0], circle[17, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[30, 0], circle[30, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[31, 0], circle[31, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[31, 0], circle[31, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[30, 0], circle[30, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[30, 0], circle[30, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[31, 0], circle[31, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[29, 0], circle[29, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[30, 0], circle[30, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[30, 0], circle[30, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[29, 0], circle[29, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[29, 0], circle[29, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[30, 0], circle[30, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[28, 0], circle[28, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[29, 0], circle[29, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[29, 0], circle[29, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[28, 0], circle[28, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[28, 0], circle[28, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[29, 0], circle[29, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[27, 0], circle[27, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[28, 0], circle[28, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[28, 0], circle[28, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[27, 0], circle[27, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[27, 0], circle[27, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[28, 0], circle[28, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[26, 0], circle[26, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[27, 0], circle[27, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[27, 0], circle[27, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[26, 0], circle[26, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[26, 0], circle[26, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[27, 0], circle[27, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[25, 0], circle[25, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[26, 0], circle[26, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[26, 0], circle[26, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[25, 0], circle[25, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[25, 0], circle[25, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[26, 0], circle[26, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[24, 0], circle[24, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[25, 0], circle[25, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[25, 0], circle[25, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[24, 0], circle[24, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[24, 0], circle[24, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[25, 0], circle[25, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[9, 0], circle[9, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[24, 0], circle[24, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[24, 0], circle[24, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[9, 0], circle[9, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[9, 0], circle[9, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[24, 0], circle[24, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[10, 0], circle[10, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[9, 0], circle[9, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[9, 0], circle[9, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[10, 0], circle[10, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[10, 0], circle[10, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[9, 0], circle[9, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[11, 0], circle[11, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[10, 0], circle[10, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[10, 0], circle[10, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[11, 0], circle[11, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[11, 0], circle[11, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[10, 0], circle[10, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[12, 0], circle[12, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[11, 0], circle[11, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[11, 0], circle[11, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[12, 0], circle[12, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[12, 0], circle[12, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[11, 0], circle[11, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[13, 0], circle[13, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[12, 0], circle[12, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[12, 0], circle[12, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[13, 0], circle[13, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[13, 0], circle[13, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[12, 0], circle[12, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[14, 0], circle[14, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[13, 0], circle[13, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[13, 0], circle[13, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[14, 0], circle[14, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[14, 0], circle[14, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[13, 0], circle[13, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[15, 0], circle[15, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[14, 0], circle[14, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[14, 0], circle[14, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[15, 0], circle[15, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[15, 0], circle[15, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[14, 0], circle[14, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[16, 0], circle[16, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[15, 0], circle[15, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[15, 0], circle[15, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[16, 0], circle[16, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[16, 0], circle[16, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[15, 0], circle[15, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[0, 0], circle[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[16, 0], circle[16, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[16, 0], circle[16, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[0, 0], circle[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[0, 0], circle[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[16, 0], circle[16, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[1, 0], circle[1, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[0, 0], circle[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[0, 0], circle[0, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[1, 0], circle[1, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[1, 0], circle[1, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[0, 0], circle[0, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[2, 0], circle[2, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[1, 0], circle[1, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[1, 0], circle[1, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[2, 0], circle[2, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[2, 0], circle[2, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[1, 0], circle[1, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[3, 0], circle[3, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[2, 0], circle[2, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[2, 0], circle[2, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[3, 0], circle[3, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[3, 0], circle[3, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[2, 0], circle[2, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[4, 0], circle[4, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[3, 0], circle[3, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[3, 0], circle[3, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[4, 0], circle[4, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[4, 0], circle[4, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[3, 0], circle[3, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[5, 0], circle[5, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[4, 0], circle[4, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[4, 0], circle[4, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[5, 0], circle[5, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[5, 0], circle[5, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[4, 0], circle[4, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[6, 0], circle[6, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[5, 0], circle[5, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[5, 0], circle[5, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[6, 0], circle[6, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[6, 0], circle[6, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[5, 0], circle[5, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[7, 0], circle[7, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[6, 0], circle[6, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[6, 0], circle[6, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[7, 0], circle[7, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[7, 0], circle[7, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[6, 0], circle[6, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[8, 0], circle[8, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[7, 0], circle[7, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[7, 0], circle[7, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[8, 0], circle[8, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[8, 0], circle[8, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[7, 0], circle[7, 1], -nol, Color.Blue.ToArgb()); i++;

            //circle2 connections
            verts[i] = new CustomVertex.PositionColored(-circle[23, 0], -circle[23, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[8, 0], -circle[8, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[8, 0], -circle[8, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[23, 0], -circle[23, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[23, 0], -circle[23, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[8, 0], -circle[8, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[22, 0], -circle[22, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[23, 0], -circle[23, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[23, 0], -circle[23, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[22, 0], -circle[22, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[22, 0], -circle[22, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[23, 0], -circle[23, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[21, 0], -circle[21, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[22, 0], -circle[22, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[22, 0], -circle[22, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[21, 0], -circle[21, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[21, 0], -circle[21, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[22, 0], -circle[22, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[20, 0], -circle[20, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[21, 0], -circle[21, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[21, 0], -circle[21, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[20, 0], -circle[20, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[20, 0], -circle[20, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[21, 0], -circle[21, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[19, 0], -circle[19, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[20, 0], -circle[20, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[20, 0], -circle[20, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[19, 0], -circle[19, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[19, 0], -circle[19, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[20, 0], -circle[20, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[18, 0], -circle[18, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[19, 0], -circle[19, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[19, 0], -circle[19, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[18, 0], -circle[18, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[18, 0], -circle[18, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[19, 0], -circle[19, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[17, 0], -circle[17, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[18, 0], -circle[18, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[18, 0], -circle[18, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[17, 0], -circle[17, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[17, 0], -circle[17, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[18, 0], -circle[18, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[31, 0], -circle[31, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[17, 0], -circle[17, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[17, 0], -circle[17, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[31, 0], -circle[31, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[31, 0], -circle[31, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[17, 0], -circle[17, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[30, 0], -circle[30, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[31, 0], -circle[31, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[31, 0], -circle[31, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[30, 0], -circle[30, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[30, 0], -circle[30, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[31, 0], -circle[31, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[29, 0], -circle[29, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[30, 0], -circle[30, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[30, 0], -circle[30, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[29, 0], -circle[29, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[29, 0], -circle[29, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[30, 0], -circle[30, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[28, 0], -circle[28, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[29, 0], -circle[29, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[29, 0], -circle[29, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[28, 0], -circle[28, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[28, 0], -circle[28, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[29, 0], -circle[29, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[27, 0], -circle[27, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[28, 0], -circle[28, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[28, 0], -circle[28, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[27, 0], -circle[27, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[27, 0], -circle[27, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[28, 0], -circle[28, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[26, 0], -circle[26, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[27, 0], -circle[27, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[27, 0], -circle[27, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[26, 0], -circle[26, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[26, 0], -circle[26, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[27, 0], -circle[27, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[25, 0], -circle[25, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[26, 0], -circle[26, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[26, 0], -circle[26, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[25, 0], -circle[25, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[25, 0], -circle[25, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[26, 0], -circle[26, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[24, 0], -circle[24, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[25, 0], -circle[25, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[25, 0], -circle[25, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[24, 0], -circle[24, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[24, 0], -circle[24, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[25, 0], -circle[25, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[9, 0], -circle[9, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[24, 0], -circle[24, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[24, 0], -circle[24, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[9, 0], -circle[9, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[9, 0], -circle[9, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[24, 0], -circle[24, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[10, 0], -circle[10, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[9, 0], -circle[9, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[9, 0], -circle[9, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[10, 0], -circle[10, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[10, 0], -circle[10, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[9, 0], -circle[9, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[11, 0], -circle[11, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[10, 0], -circle[10, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[10, 0], -circle[10, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[11, 0], -circle[11, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[11, 0], -circle[11, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[10, 0], -circle[10, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[12, 0], -circle[12, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[11, 0], -circle[11, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[11, 0], -circle[11, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[12, 0], -circle[12, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[12, 0], -circle[12, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[11, 0], -circle[11, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[13, 0], -circle[13, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[12, 0], -circle[12, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[12, 0], -circle[12, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[13, 0], -circle[13, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[13, 0], -circle[13, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[12, 0], -circle[12, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[14, 0], -circle[14, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[13, 0], -circle[13, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[13, 0], -circle[13, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[14, 0], -circle[14, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[14, 0], -circle[14, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[13, 0], -circle[13, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[15, 0], -circle[15, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[14, 0], -circle[14, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[14, 0], -circle[14, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[15, 0], -circle[15, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[15, 0], -circle[15, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[14, 0], -circle[14, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[16, 0], -circle[16, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[15, 0], -circle[15, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[15, 0], -circle[15, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[16, 0], -circle[16, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[16, 0], -circle[16, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[15, 0], -circle[15, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[0, 0], -circle[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[16, 0], -circle[16, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[16, 0], -circle[16, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[0, 0], -circle[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[0, 0], -circle[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[16, 0], -circle[16, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[1, 0], -circle[1, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[0, 0], -circle[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[0, 0], -circle[0, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[1, 0], -circle[1, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[1, 0], -circle[1, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[0, 0], -circle[0, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[2, 0], -circle[2, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[1, 0], -circle[1, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[1, 0], -circle[1, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[2, 0], -circle[2, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[2, 0], -circle[2, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[1, 0], -circle[1, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[3, 0], -circle[3, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[2, 0], -circle[2, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[2, 0], -circle[2, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[3, 0], -circle[3, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[3, 0], -circle[3, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[2, 0], -circle[2, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[4, 0], -circle[4, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[3, 0], -circle[3, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[3, 0], -circle[3, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[4, 0], -circle[4, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[4, 0], -circle[4, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[3, 0], -circle[3, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[5, 0], -circle[5, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[4, 0], -circle[4, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[4, 0], -circle[4, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[5, 0], -circle[5, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[5, 0], -circle[5, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[4, 0], -circle[4, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[6, 0], -circle[6, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[5, 0], -circle[5, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[5, 0], -circle[5, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[6, 0], -circle[6, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[6, 0], -circle[6, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[5, 0], -circle[5, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[7, 0], -circle[7, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[6, 0], -circle[6, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[6, 0], -circle[6, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[7, 0], -circle[7, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[7, 0], -circle[7, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[6, 0], -circle[6, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[8, 0], -circle[8, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[7, 0], -circle[7, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[7, 0], -circle[7, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[8, 0], -circle[8, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[8, 0], -circle[8, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[7, 0], -circle[7, 1], -nol, Color.Blue.ToArgb()); i++;

            //circle
            verts[i] = new CustomVertex.PositionColored(circle[1, 0], circle[1, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[0, 0], circle[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[17, 0], circle[17, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[1, 0], circle[1, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[17, 0], circle[17, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[18, 0], circle[18, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[2, 0], circle[2, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[1, 0], circle[1, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[18, 0], circle[18, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[2, 0], circle[2, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[18, 0], circle[18, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[19, 0], circle[19, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[3, 0], circle[3, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[2, 0], circle[2, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[19, 0], circle[19, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[3, 0], circle[3, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[19, 0], circle[19, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[20, 0], circle[20, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[4, 0], circle[4, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[3, 0], circle[3, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[20, 0], circle[20, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[4, 0], circle[4, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[20, 0], circle[20, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[21, 0], circle[21, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[5, 0], circle[5, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[21, 0], circle[21, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[22, 0], circle[22, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[5, 0], circle[5, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[4, 0], circle[4, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[21, 0], circle[21, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[6, 0], circle[6, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[5, 0], circle[5, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[22, 0], circle[22, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[6, 0], circle[6, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[22, 0], circle[22, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[23, 0], circle[23, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[7, 0], circle[7, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[6, 0], circle[6, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[23, 0], circle[23, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[8, 0], circle[8, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[7, 0], circle[7, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[23, 0], circle[23, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[0, 0], circle[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[16, 0], circle[16, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[31, 0], circle[31, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[0, 0], circle[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[31, 0], circle[31, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[17, 0], circle[17, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[16, 0], circle[16, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[30, 0], circle[30, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[31, 0], circle[31, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[16, 0], circle[16, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[15, 0], circle[15, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[30, 0], circle[30, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[15, 0], circle[15, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[29, 0], circle[29, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[30, 0], circle[30, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[15, 0], circle[15, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[14, 0], circle[14, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[29, 0], circle[29, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[14, 0], circle[14, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[28, 0], circle[28, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[29, 0], circle[29, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[14, 0], circle[14, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[13, 0], circle[13, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[28, 0], circle[28, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[13, 0], circle[13, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[27, 0], circle[27, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[28, 0], circle[28, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[13, 0], circle[13, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[12, 0], circle[12, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[27, 0], circle[27, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[12, 0], circle[12, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[26, 0], circle[26, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[27, 0], circle[27, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[12, 0], circle[12, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[11, 0], circle[11, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[26, 0], circle[26, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[11, 0], circle[11, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[25, 0], circle[25, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[26, 0], circle[26, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[11, 0], circle[11, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[10, 0], circle[10, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[25, 0], circle[25, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[10, 0], circle[10, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[24, 0], circle[24, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[25, 0], circle[25, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[10, 0], circle[10, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[9, 0], circle[9, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[24, 0], circle[24, 1], nol, Color.Blue.ToArgb()); i++;

            //plane

            verts[i] = new CustomVertex.PositionColored(plane[0, 0], plane[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane[3, 0], plane[3, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane[1, 0], plane[1, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane[3, 0], plane[3, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane[2, 0], plane[2, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane[1, 0], plane[1, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane[5, 0], plane[5, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane[6, 0], plane[6, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane[7, 0], plane[7, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane[5, 0], plane[5, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane[7, 0], plane[7, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane[4, 0], plane[4, 1], nol, Color.Blue.ToArgb()); i++;

            //cirle 2
            verts[i] = new CustomVertex.PositionColored(-circle[1, 0], -circle[1, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[0, 0], -circle[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[17, 0], -circle[17, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[1, 0], -circle[1, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[17, 0], -circle[17, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[18, 0], -circle[18, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[2, 0], -circle[2, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[1, 0], -circle[1, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[18, 0], -circle[18, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[2, 0], -circle[2, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[18, 0], -circle[18, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[19, 0], -circle[19, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[3, 0], -circle[3, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[2, 0], -circle[2, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[19, 0], -circle[19, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[3, 0], -circle[3, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[19, 0], -circle[19, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[20, 0], -circle[20, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[4, 0], -circle[4, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[3, 0], -circle[3, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[20, 0], -circle[20, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[4, 0], -circle[4, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[20, 0], -circle[20, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[21, 0], -circle[21, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[5, 0], -circle[5, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[21, 0], -circle[21, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[22, 0], -circle[22, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[5, 0], -circle[5, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[4, 0], -circle[4, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[21, 0], -circle[21, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[6, 0], -circle[6, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[5, 0], -circle[5, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[22, 0], -circle[22, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[6, 0], -circle[6, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[22, 0], -circle[22, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[23, 0], -circle[23, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[7, 0], -circle[7, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[6, 0], -circle[6, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[23, 0], -circle[23, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[8, 0], -circle[8, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[7, 0], -circle[7, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[23, 0], -circle[23, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[0, 0], -circle[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[16, 0], -circle[16, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[31, 0], -circle[31, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[0, 0], -circle[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[31, 0], -circle[31, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[17, 0], -circle[17, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[16, 0], -circle[16, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[30, 0], -circle[30, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[31, 0], -circle[31, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[16, 0], -circle[16, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[15, 0], -circle[15, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[30, 0], -circle[30, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[15, 0], -circle[15, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[29, 0], -circle[29, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[30, 0], -circle[30, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[15, 0], -circle[15, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[14, 0], -circle[14, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[29, 0], -circle[29, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[14, 0], -circle[14, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[28, 0], -circle[28, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[29, 0], -circle[29, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[14, 0], -circle[14, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[13, 0], -circle[13, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[28, 0], -circle[28, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[13, 0], -circle[13, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[27, 0], -circle[27, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[28, 0], -circle[28, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[13, 0], -circle[13, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[12, 0], -circle[12, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[27, 0], -circle[27, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[12, 0], -circle[12, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[26, 0], -circle[26, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[27, 0], -circle[27, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[12, 0], -circle[12, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[11, 0], -circle[11, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[26, 0], -circle[26, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[11, 0], -circle[11, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[25, 0], -circle[25, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[26, 0], -circle[26, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[11, 0], -circle[11, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[10, 0], -circle[10, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[25, 0], -circle[25, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[10, 0], -circle[10, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[24, 0], -circle[24, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[25, 0], -circle[25, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[10, 0], -circle[10, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[9, 0], -circle[9, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[24, 0], -circle[24, 1], nol, Color.Blue.ToArgb()); i++;


            //plane1 #1
            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], plane1[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[1, 0], plane1[1, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[4, 0], plane1[4, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], plane1[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[4, 0], plane1[4, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[3, 0], plane1[3, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[1, 0], plane1[1, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[2, 0], plane1[2, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[4, 0], plane1[4, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], plane1[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[3, 0], plane1[3, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[5, 0], plane1[5, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], plane1[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[5, 0], plane1[5, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[6, 0], plane1[6, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], plane1[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[6, 0], plane1[6, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[8, 0], plane1[8, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], plane1[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[8, 0], plane1[8, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[7, 0], plane1[7, 1], nol, Color.Blue.ToArgb()); i++;

            //plane1 #2
            verts[i] = new CustomVertex.PositionColored(-plane1[4, 0], plane1[4, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[1, 0], plane1[1, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], plane1[0, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[3, 0], plane1[3, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[4, 0], plane1[4, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], plane1[0, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[4, 0], plane1[4, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[2, 0], plane1[2, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[1, 0], plane1[1, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[5, 0], plane1[5, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[3, 0], plane1[3, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], plane1[0, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[6, 0], plane1[6, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[5, 0], plane1[5, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], plane1[0, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[8, 0], plane1[8, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[6, 0], plane1[6, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], plane1[0, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[7, 0], plane1[7, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[8, 0], plane1[8, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], plane1[0, 1], nol, Color.Blue.ToArgb()); i++;

            //plane1 #3

            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], -plane1[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[1, 0], -plane1[1, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[4, 0], -plane1[4, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], -plane1[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[4, 0], -plane1[4, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[3, 0], -plane1[3, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[1, 0], -plane1[1, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[2, 0], -plane1[2, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[4, 0], -plane1[4, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], -plane1[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[3, 0], -plane1[3, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[5, 0], -plane1[5, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], -plane1[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[5, 0], -plane1[5, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[6, 0], -plane1[6, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], -plane1[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[6, 0], -plane1[6, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[8, 0], -plane1[8, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], -plane1[0, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[8, 0], -plane1[8, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[7, 0], -plane1[7, 1], nol, Color.Blue.ToArgb()); i++;

            //plane1 #4
            verts[i] = new CustomVertex.PositionColored(plane1[4, 0], -plane1[4, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[1, 0], -plane1[1, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], -plane1[0, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[3, 0], -plane1[3, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[4, 0], -plane1[4, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], -plane1[0, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[4, 0], -plane1[4, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[2, 0], -plane1[2, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[1, 0], -plane1[1, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[5, 0], -plane1[5, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[3, 0], -plane1[3, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], -plane1[0, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[6, 0], -plane1[6, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[5, 0], -plane1[5, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], -plane1[0, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[8, 0], -plane1[8, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[6, 0], -plane1[6, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], -plane1[0, 1], nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[7, 0], -plane1[7, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[8, 0], -plane1[8, 1], nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], -plane1[0, 1], nol, Color.Blue.ToArgb()); i++;

            //back side
            //circle
            verts[i] = new CustomVertex.PositionColored(circle[17, 0], circle[17, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[0, 0], circle[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[1, 0], circle[1, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[18, 0], circle[18, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[17, 0], circle[17, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[1, 0], circle[1, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[18, 0], circle[18, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[1, 0], circle[1, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[2, 0], circle[2, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[19, 0], circle[19, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[18, 0], circle[18, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[2, 0], circle[2, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[19, 0], circle[19, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[2, 0], circle[2, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[3, 0], circle[3, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[20, 0], circle[20, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[19, 0], circle[19, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[3, 0], circle[3, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[20, 0], circle[20, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[3, 0], circle[3, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[4, 0], circle[4, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[20, 0], circle[20, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[4, 0], circle[4, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[21, 0], circle[21, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[22, 0], circle[22, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[21, 0], circle[21, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[5, 0], circle[5, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[21, 0], circle[21, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[4, 0], circle[4, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[5, 0], circle[5, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[22, 0], circle[22, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[5, 0], circle[5, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[6, 0], circle[6, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[23, 0], circle[23, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[22, 0], circle[22, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[6, 0], circle[6, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[23, 0], circle[23, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[6, 0], circle[6, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[7, 0], circle[7, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[23, 0], circle[23, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[7, 0], circle[7, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[8, 0], circle[8, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[31, 0], circle[31, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[16, 0], circle[16, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[0, 0], circle[0, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[17, 0], circle[17, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[31, 0], circle[31, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[0, 0], circle[0, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[31, 0], circle[31, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[30, 0], circle[30, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[16, 0], circle[16, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[30, 0], circle[30, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[15, 0], circle[15, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[16, 0], circle[16, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[30, 0], circle[30, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[29, 0], circle[29, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[15, 0], circle[15, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[29, 0], circle[29, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[14, 0], circle[14, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[15, 0], circle[15, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[29, 0], circle[29, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[28, 0], circle[28, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[14, 0], circle[14, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[28, 0], circle[28, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[13, 0], circle[13, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[14, 0], circle[14, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[28, 0], circle[28, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[27, 0], circle[27, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[13, 0], circle[13, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[27, 0], circle[27, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[12, 0], circle[12, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[13, 0], circle[13, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[27, 0], circle[27, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[26, 0], circle[26, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[12, 0], circle[12, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[26, 0], circle[26, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[11, 0], circle[11, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[12, 0], circle[12, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[26, 0], circle[26, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[25, 0], circle[25, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[11, 0], circle[11, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[25, 0], circle[25, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[10, 0], circle[10, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[11, 0], circle[11, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[25, 0], circle[25, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[24, 0], circle[24, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[10, 0], circle[10, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(circle[24, 0], circle[24, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[9, 0], circle[9, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(circle[10, 0], circle[10, 1], -nol, Color.Blue.ToArgb()); i++;

            //plane

            verts[i] = new CustomVertex.PositionColored(plane[1, 0], plane[1, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane[3, 0], plane[3, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane[0, 0], plane[0, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane[1, 0], plane[1, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane[2, 0], plane[2, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane[3, 0], plane[3, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane[7, 0], plane[7, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane[6, 0], plane[6, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane[5, 0], plane[5, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane[4, 0], plane[4, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane[7, 0], plane[7, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane[5, 0], plane[5, 1], -nol, Color.Blue.ToArgb()); i++;

            //cirle 2
            verts[i] = new CustomVertex.PositionColored(-circle[17, 0], -circle[17, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[0, 0], -circle[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[1, 0], -circle[1, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[18, 0], -circle[18, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[17, 0], -circle[17, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[1, 0], -circle[1, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[18, 0], -circle[18, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[1, 0], -circle[1, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[2, 0], -circle[2, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[19, 0], -circle[19, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[18, 0], -circle[18, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[2, 0], -circle[2, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[19, 0], -circle[19, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[2, 0], -circle[2, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[3, 0], -circle[3, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[20, 0], -circle[20, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[19, 0], -circle[19, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[3, 0], -circle[3, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[20, 0], -circle[20, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[3, 0], -circle[3, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[4, 0], -circle[4, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[21, 0], -circle[21, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[20, 0], -circle[20, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[4, 0], -circle[4, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[22, 0], -circle[22, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[21, 0], -circle[21, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[5, 0], -circle[5, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[21, 0], -circle[21, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[4, 0], -circle[4, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[5, 0], -circle[5, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[22, 0], -circle[22, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[5, 0], -circle[5, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[6, 0], -circle[6, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[23, 0], -circle[23, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[22, 0], -circle[22, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[6, 0], -circle[6, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[23, 0], -circle[23, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[6, 0], -circle[6, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[7, 0], -circle[7, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[23, 0], -circle[23, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[7, 0], -circle[7, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[8, 0], -circle[8, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[31, 0], -circle[31, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[16, 0], -circle[16, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[0, 0], -circle[0, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[17, 0], -circle[17, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[31, 0], -circle[31, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[0, 0], -circle[0, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[31, 0], -circle[31, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[30, 0], -circle[30, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[16, 0], -circle[16, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[30, 0], -circle[30, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[15, 0], -circle[15, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[16, 0], -circle[16, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[30, 0], -circle[30, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[29, 0], -circle[29, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[15, 0], -circle[15, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[29, 0], -circle[29, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[14, 0], -circle[14, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[15, 0], -circle[15, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[29, 0], -circle[29, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[28, 0], -circle[28, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[14, 0], -circle[14, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[28, 0], -circle[28, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[13, 0], -circle[13, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[14, 0], -circle[14, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[28, 0], -circle[28, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[27, 0], -circle[27, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[13, 0], -circle[13, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[27, 0], -circle[27, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[12, 0], -circle[12, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[13, 0], -circle[13, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[27, 0], -circle[27, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[26, 0], -circle[26, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[12, 0], -circle[12, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[26, 0], -circle[26, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[11, 0], -circle[11, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[12, 0], -circle[12, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[26, 0], -circle[26, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[25, 0], -circle[25, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[11, 0], -circle[11, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[25, 0], -circle[25, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[10, 0], -circle[10, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[11, 0], -circle[11, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[25, 0], -circle[25, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[24, 0], -circle[24, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[10, 0], -circle[10, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-circle[24, 0], -circle[24, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[9, 0], -circle[9, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-circle[10, 0], -circle[10, 1], -nol, Color.Blue.ToArgb()); i++;


            //plane1 #1
            verts[i] = new CustomVertex.PositionColored(plane1[4, 0], plane1[4, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[1, 0], plane1[1, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[3, 0], plane1[3, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[4, 0], plane1[4, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[4, 0], plane1[4, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[2, 0], plane1[2, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[1, 0], plane1[1, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[5, 0], plane1[5, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[3, 0], plane1[3, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[6, 0], plane1[6, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[5, 0], plane1[5, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[8, 0], plane1[8, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[6, 0], plane1[6, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[7, 0], plane1[7, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[8, 0], plane1[8, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;

            //plane1 #2
            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[1, 0], plane1[1, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[4, 0], plane1[4, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[4, 0], plane1[4, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[3, 0], plane1[3, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[1, 0], plane1[1, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[2, 0], plane1[2, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[4, 0], plane1[4, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[3, 0], plane1[3, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[5, 0], plane1[5, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[5, 0], plane1[5, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[6, 0], plane1[6, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[6, 0], plane1[6, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[8, 0], plane1[8, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[8, 0], plane1[8, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[7, 0], plane1[7, 1], -nol, Color.Blue.ToArgb()); i++;

            //plane1 #3

            verts[i] = new CustomVertex.PositionColored(-plane1[4, 0], -plane1[4, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[1, 0], -plane1[1, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], -plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[3, 0], -plane1[3, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[4, 0], -plane1[4, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], -plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[4, 0], -plane1[4, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[2, 0], -plane1[2, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[1, 0], -plane1[1, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[5, 0], -plane1[5, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[3, 0], -plane1[3, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], -plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[6, 0], -plane1[6, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[5, 0], -plane1[5, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], -plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[8, 0], -plane1[8, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[6, 0], -plane1[6, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], -plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(-plane1[7, 0], -plane1[7, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[8, 0], -plane1[8, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(-plane1[0, 0], -plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;

            //plane1 #4
            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], -plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[1, 0], -plane1[1, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[4, 0], -plane1[4, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], -plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[4, 0], -plane1[4, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[3, 0], -plane1[3, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[1, 0], -plane1[1, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[2, 0], -plane1[2, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[4, 0], -plane1[4, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], -plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[3, 0], -plane1[3, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[5, 0], -plane1[5, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], -plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[5, 0], -plane1[5, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[6, 0], -plane1[6, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], -plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[6, 0], -plane1[6, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[8, 0], -plane1[8, 1], -nol, Color.Blue.ToArgb()); i++;

            verts[i] = new CustomVertex.PositionColored(plane1[0, 0], -plane1[0, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[8, 0], -plane1[8, 1], -nol, Color.Blue.ToArgb()); i++;
            verts[i] = new CustomVertex.PositionColored(plane1[7, 0], -plane1[7, 1], -nol, Color.Blue.ToArgb()); i++;

            buffer.SetData(verts, 0, LockFlags.None);
        }

        private void SetupCamera()
        {
            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI/4, this.Width/this.Height, 1.0f, 100.0f);
            device.Transform.View = Matrix.LookAtLH(new Vector3(0, 0, 50), new Vector3(), new Vector3(0, 1, 0));

            //device.Transform.World = Matrix.RotationX(angle) * Matrix.RotationY(angle * 2) * Matrix.RotationZ(angle * .7f);
            device.Transform.World = Matrix.RotationY(angle);
            angle += 0.02f;

            device.RenderState.Lighting = false;
            device.RenderState.CullMode = Cull.CounterClockwise;
        }

        private void ScreenSaverForm_Paint(object sender, PaintEventArgs e)
        {
            device.Clear(ClearFlags.Target, Color.Black, 1, 0);

            SetupCamera();

            device.BeginScene();

            device.VertexFormat = CustomVertex.PositionColored.Format;
            device.SetStreamSource(0, vb, 0);
            device.DrawPrimitives(PrimitiveType.TriangleList, 0, 376);

            device.EndScene();

            device.Present();

            this.Invalidate();
        }

        public ScreenSaverForm(System.Drawing.Rectangle Bounds)
        {
            InitializeComponent();
            this.Bounds = Bounds;
        }


        private void ScreenSaverForm_Load(object sender, EventArgs e)
        {
            Cursor.Hide();
            TopMost = true;
        }

        private void ScreenSaverForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseLocation.IsEmpty)
            {
                // Terminate if mouse is moved a significant distance
                if (Math.Abs(mouseLocation.X - e.X) > 5 ||
                    Math.Abs(mouseLocation.Y - e.Y) > 5)
                    Application.Exit();
            }

            // Update current mouse location
            mouseLocation = e.Location;
        }

        private void ScreenSaverForm_MouseClick(object sender, MouseEventArgs e)
        {
            Application.Exit();
        }

        private void ScreenSaverForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            Application.Exit();
        }
    }
}
