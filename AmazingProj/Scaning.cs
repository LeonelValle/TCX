﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCX
{
    public partial class Scaning : Form
    {
        WorkOrder wo = new WorkOrder();
        Scan scan = new Scan();
        User user = new User();

        public Scaning()
        {
            InitializeComponent();
        }
        private void Scaning_Load(object sender, EventArgs e)
        {
            dg_Scan.DataSource = scan.LlenarDG("select sn, dateReg, pn.pn, wo.rev from tb_Scan sc join tb_WO wo on sc.id_wo = wo.id_wo join tb_PN pn on pn.id_pn = wo.id_pn where wo.id_wo = '" + wo.Id_wo + "'").Tables[0];

            lbl_Employ.Text = user.ReturnValue("select idemp from tb_User where id_user = '" + user.Id_user + "'");
            lbl_WO.Text = user.ReturnValue("select wo from tb_WO where id_wo = '" + wo.Id_wo + "'");
            lbl_Records.Text = scan.ReturnValue("select count(*) from tb_Scan where id_wo = '" + wo.Id_wo + "'");
            lbl_Firm.Text = scan.ReturnValue("select rev from tb_WO where id_wo = '" + wo.Id_wo + "'");
            lbl_Rev.Text = scan.ReturnValue("select firmware from tb_WO where id_wo = '" + wo.Id_wo + "'");

            txt_Scan.Focus();

            if (int.Parse(scan.ReturnValue("select count(*) from tb_Scan where id_wo = '" + wo.Id_wo + "'")) >= int.Parse(wo.ReturnValue("select qty from tb_WO where id_wo = '" + wo.Id_wo + "'")))
            {
                txt_Scan.Enabled = false;
                MessageBox.Show("WO COMPLETED!");
            }

        }

        private void btn_Submit_Click(object sender, EventArgs e)
        {
            if (int.Parse(scan.ReturnValue("select count(*) from tb_Scan where id_wo = '" + wo.Id_wo + "'")) >= int.Parse(wo.ReturnValue("select qty from tb_WO where id_wo = '" + wo.Id_wo + "'")))
            {
                txt_Scan.Enabled = false;
                MessageBox.Show("WO COMPLETED!");
            }
            else
            {
                if (!string.IsNullOrEmpty(txt_Scan.Text) || !string.IsNullOrEmpty(txt_Box.Text))
                {
                    if (txt_Scan.Text.Trim().Length == 18 || txt_Box.Text.Trim().Length == 18)
                    {
                        if (txt_Scan.Text.Trim() == txt_Box.Text.Trim())
                        {

                            if (!scan.Existe("select count(*) from tb_Scan where sn = '" + txt_Scan.Text + "'"))
                            {
                                scan.Crud("insert into tb_Scan values('" + txt_Scan.Text.Trim() + "','" + DateTime.Now + "','" + wo.Id_wo + "','" + user.Id_user + "')"); ;
                                txt_Scan.Text = "";
                                txt_Box.Text = "";
                                txt_Scan.Focus();
                                pictureBox1.Image = TCX.Properties.Resources._69_692608_transparent_answer_icon_png_check_pass_icon_png;
                                Scaning_Load(sender, e);

                            }
                            else
                            {
                                MessageBox.Show("This unit already exist!");
                                txt_Scan.Text = "";
                                txt_Box.Text = "";
                                txt_Scan.Focus();
                            }
                        }
                        else
                        {
                            MessageBox.Show("the serial number are not the same!");
                            pictureBox1.Image = TCX.Properties.Resources.bad;
                            txt_Scan.Focus();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Check Serial Number!");
                        pictureBox1.Image = TCX.Properties.Resources.bad;
                        txt_Scan.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Insert an unit!");
                    txt_Scan.Focus();
                }

            }
        }

        private void btn_Submit_Enter(object sender, EventArgs e) =>
            SendKeys.Send("{ENTER}");

    }
}
