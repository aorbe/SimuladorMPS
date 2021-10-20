using System;
using System.Drawing;
using System.Windows.Forms;

namespace Simulador_MPS { 
partial class SimuladorForm : Form
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimuladorForm));
            this.btnUpPressure = new System.Windows.Forms.Button();
            this.btnDownPressure = new System.Windows.Forms.Button();
            this.btnPartida = new System.Windows.Forms.Button();
            this.lblConnections = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblPressao = new System.Windows.Forms.Label();
            this.btnPeca = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnEst12 = new System.Windows.Forms.Button();
            this.btnInfo = new System.Windows.Forms.Button();
            this.btnEst34 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnUpPressure
            // 
            this.btnUpPressure.Location = new System.Drawing.Point(5, 19);
            this.btnUpPressure.Name = "btnUpPressure";
            this.btnUpPressure.Size = new System.Drawing.Size(70, 24);
            this.btnUpPressure.TabIndex = 0;
            this.btnUpPressure.Text = "+";
            this.btnUpPressure.UseVisualStyleBackColor = true;
            this.btnUpPressure.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnDownPressure
            // 
            this.btnDownPressure.Location = new System.Drawing.Point(74, 19);
            this.btnDownPressure.Name = "btnDownPressure";
            this.btnDownPressure.Size = new System.Drawing.Size(68, 24);
            this.btnDownPressure.TabIndex = 1;
            this.btnDownPressure.Text = "-";
            this.btnDownPressure.UseVisualStyleBackColor = true;
            this.btnDownPressure.Click += new System.EventHandler(this.btnDownPressure_Click);
            // 
            // btnPartida
            // 
            this.btnPartida.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPartida.Location = new System.Drawing.Point(148, 5);
            this.btnPartida.Name = "btnPartida";
            this.btnPartida.Size = new System.Drawing.Size(137, 38);
            this.btnPartida.TabIndex = 2;
            this.btnPartida.Text = "PARTIDA";
            this.btnPartida.UseVisualStyleBackColor = true;
            this.btnPartida.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPartida_MouseDown);
            this.btnPartida.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnPartida_MouseUp);
            // 
            // lblConnections
            // 
            this.lblConnections.AutoSize = true;
            this.lblConnections.Location = new System.Drawing.Point(100, 191);
            this.lblConnections.Name = "lblConnections";
            this.lblConnections.Size = new System.Drawing.Size(13, 13);
            this.lblConnections.TabIndex = 9;
            this.lblConnections.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 191);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Conexões Ativas";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Pressão (bar)";
            // 
            // lblPressao
            // 
            this.lblPressao.AutoSize = true;
            this.lblPressao.Location = new System.Drawing.Point(100, 3);
            this.lblPressao.Name = "lblPressao";
            this.lblPressao.Size = new System.Drawing.Size(13, 13);
            this.lblPressao.TabIndex = 12;
            this.lblPressao.Text = "0";
            // 
            // btnPeca
            // 
            this.btnPeca.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPeca.Location = new System.Drawing.Point(5, 49);
            this.btnPeca.Name = "btnPeca";
            this.btnPeca.Size = new System.Drawing.Size(137, 38);
            this.btnPeca.TabIndex = 13;
            this.btnPeca.Text = "INSERIR PEÇA";
            this.btnPeca.UseVisualStyleBackColor = true;
            this.btnPeca.Click += new System.EventHandler(this.btnPeca_Click);
            // 
            // btnReset
            // 
            this.btnReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.Location = new System.Drawing.Point(148, 49);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(137, 38);
            this.btnReset.TabIndex = 34;
            this.btnReset.Text = "REMOVER PEÇAS";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnEst12
            // 
            this.btnEst12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEst12.Location = new System.Drawing.Point(291, 5);
            this.btnEst12.Name = "btnEst12";
            this.btnEst12.Size = new System.Drawing.Size(94, 38);
            this.btnEst12.TabIndex = 47;
            this.btnEst12.Text = "1 - 2";
            this.btnEst12.UseVisualStyleBackColor = true;
            this.btnEst12.Click += new System.EventHandler(this.btnEst12_Click);
            // 
            // btnInfo
            // 
            this.btnInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnInfo.BackgroundImage = global::Simulador_MPS.Properties.Resources.Info_Image;
            this.btnInfo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInfo.Location = new System.Drawing.Point(321, 155);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(64, 46);
            this.btnInfo.TabIndex = 44;
            this.btnInfo.UseVisualStyleBackColor = false;
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
            // 
            // btnEst34
            // 
            this.btnEst34.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEst34.Location = new System.Drawing.Point(291, 49);
            this.btnEst34.Name = "btnEst34";
            this.btnEst34.Size = new System.Drawing.Size(94, 38);
            this.btnEst34.TabIndex = 48;
            this.btnEst34.Text = "3 - 4";
            this.btnEst34.UseVisualStyleBackColor = true;
            this.btnEst34.Click += new System.EventHandler(this.btnEst34_Click);
            // 
            // SimuladorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(391, 213);
            this.Controls.Add(this.btnEst34);
            this.Controls.Add(this.btnEst12);
            this.Controls.Add(this.btnInfo);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnPeca);
            this.Controls.Add(this.lblPressao);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblConnections);
            this.Controls.Add(this.btnPartida);
            this.Controls.Add(this.btnDownPressure);
            this.Controls.Add(this.btnUpPressure);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SimuladorForm";
            this.Text = "Simulador MPS SENAI";
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnUpPressure;
    private System.Windows.Forms.Button btnDownPressure;
    private System.Windows.Forms.Button btnPartida;
    private System.Windows.Forms.Label lblConnections;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label lblPressao;
    private System.Windows.Forms.Button btnPeca;
    private System.Windows.Forms.Button btnReset;
        private Button btnInfo;
        private Button btnEst12;
        private Button btnEst34;
    }
}