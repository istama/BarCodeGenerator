<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
  Inherits System.Windows.Forms.Form

  'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
  <System.Diagnostics.DebuggerNonUserCode()> _
  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    Try
      If disposing AndAlso components IsNot Nothing Then
        components.Dispose()
      End If
    Finally
      MyBase.Dispose(disposing)
    End Try
  End Sub

  'Windows フォーム デザイナーで必要です。
  Private components As System.ComponentModel.IContainer

  'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
  'Windows フォーム デザイナーを使用して変更できます。  
  'コード エディターを使って変更しないでください。
  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
    Me.txtPrefix = New System.Windows.Forms.TextBox()
    Me.txtCode = New System.Windows.Forms.TextBox()
    Me.txtSufix = New System.Windows.Forms.TextBox()
    Me.Label1 = New System.Windows.Forms.Label()
    Me.Label2 = New System.Windows.Forms.Label()
    Me.Label3 = New System.Windows.Forms.Label()
    Me.cboxOutputCnt = New System.Windows.Forms.ComboBox()
    Me.Label4 = New System.Windows.Forms.Label()
    Me.cmdAddCode = New System.Windows.Forms.Button()
    Me.lboxOutputCode = New System.Windows.Forms.ListBox()
    Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
    Me.設定ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
    Me.印刷ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
    Me.ページ設定ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
    Me.印刷プレビューToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
    Me.印刷ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
    Me.Label5 = New System.Windows.Forms.Label()
    Me.cmdCodeRemove = New System.Windows.Forms.Button()
    Me.cmdCodeClear = New System.Windows.Forms.Button()
    Me.cmdOutput = New System.Windows.Forms.Button()
    Me.cmdPreview = New System.Windows.Forms.Button()
    Me.Label6 = New System.Windows.Forms.Label()
    Me.label7 = New System.Windows.Forms.Label()
    Me.cboxCodeLength = New System.Windows.Forms.ComboBox()
    Me.lboxOutputCnt = New System.Windows.Forms.ListBox()
    Me.Label8 = New System.Windows.Forms.Label()
    Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument()
    Me.PrintDialog1 = New System.Windows.Forms.PrintDialog()
    Me.MenuStrip1.SuspendLayout()
    Me.SuspendLayout()
    '
    'txtPrefix
    '
    Me.txtPrefix.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
    Me.txtPrefix.Location = New System.Drawing.Point(82, 64)
    Me.txtPrefix.Name = "txtPrefix"
    Me.txtPrefix.Size = New System.Drawing.Size(156, 26)
    Me.txtPrefix.TabIndex = 12
    Me.txtPrefix.Text = "A"
    '
    'txtCode
    '
    Me.txtCode.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
    Me.txtCode.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
    Me.txtCode.Location = New System.Drawing.Point(82, 97)
    Me.txtCode.Name = "txtCode"
    Me.txtCode.Size = New System.Drawing.Size(156, 26)
    Me.txtCode.TabIndex = 0
    '
    'txtSufix
    '
    Me.txtSufix.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
    Me.txtSufix.Location = New System.Drawing.Point(82, 130)
    Me.txtSufix.Name = "txtSufix"
    Me.txtSufix.Size = New System.Drawing.Size(156, 26)
    Me.txtSufix.TabIndex = 1
    Me.txtSufix.Text = "A"
    '
    'Label1
    '
    Me.Label1.AutoSize = True
    Me.Label1.Location = New System.Drawing.Point(10, 71)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(41, 12)
    Me.Label1.TabIndex = 3
    Me.Label1.Text = "接頭辞"
    '
    'Label2
    '
    Me.Label2.AutoSize = True
    Me.Label2.Location = New System.Drawing.Point(10, 102)
    Me.Label2.Name = "Label2"
    Me.Label2.Size = New System.Drawing.Size(32, 12)
    Me.Label2.TabIndex = 4
    Me.Label2.Text = "コード"
    '
    'Label3
    '
    Me.Label3.AutoSize = True
    Me.Label3.Location = New System.Drawing.Point(10, 137)
    Me.Label3.Name = "Label3"
    Me.Label3.Size = New System.Drawing.Size(41, 12)
    Me.Label3.TabIndex = 5
    Me.Label3.Text = "接尾辞"
    '
    'cboxOutputCnt
    '
    Me.cboxOutputCnt.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
    Me.cboxOutputCnt.FormattingEnabled = True
    Me.cboxOutputCnt.Items.AddRange(New Object() {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20"})
    Me.cboxOutputCnt.Location = New System.Drawing.Point(82, 166)
    Me.cboxOutputCnt.Name = "cboxOutputCnt"
    Me.cboxOutputCnt.Size = New System.Drawing.Size(121, 24)
    Me.cboxOutputCnt.TabIndex = 3
    Me.cboxOutputCnt.Text = "1"
    '
    'Label4
    '
    Me.Label4.AutoSize = True
    Me.Label4.Location = New System.Drawing.Point(10, 172)
    Me.Label4.Name = "Label4"
    Me.Label4.Size = New System.Drawing.Size(53, 12)
    Me.Label4.TabIndex = 7
    Me.Label4.Text = "出力件数"
    '
    'cmdAddCode
    '
    Me.cmdAddCode.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
    Me.cmdAddCode.Location = New System.Drawing.Point(254, 92)
    Me.cmdAddCode.Name = "cmdAddCode"
    Me.cmdAddCode.Size = New System.Drawing.Size(49, 37)
    Me.cmdAddCode.TabIndex = 5
    Me.cmdAddCode.Text = "=>"
    Me.cmdAddCode.UseVisualStyleBackColor = True
    '
    'lboxOutputCode
    '
    Me.lboxOutputCode.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
    Me.lboxOutputCode.FormattingEnabled = True
    Me.lboxOutputCode.ItemHeight = 16
    Me.lboxOutputCode.Location = New System.Drawing.Point(319, 64)
    Me.lboxOutputCode.Name = "lboxOutputCode"
    Me.lboxOutputCode.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
    Me.lboxOutputCode.Size = New System.Drawing.Size(146, 244)
    Me.lboxOutputCode.TabIndex = 7
    '
    'MenuStrip1
    '
    Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.設定ToolStripMenuItem, Me.印刷ToolStripMenuItem})
    Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
    Me.MenuStrip1.Name = "MenuStrip1"
    Me.MenuStrip1.Size = New System.Drawing.Size(532, 26)
    Me.MenuStrip1.TabIndex = 10
    Me.MenuStrip1.Text = "MenuStrip1"
    '
    '設定ToolStripMenuItem
    '
    Me.設定ToolStripMenuItem.Name = "設定ToolStripMenuItem"
    Me.設定ToolStripMenuItem.Size = New System.Drawing.Size(44, 22)
    Me.設定ToolStripMenuItem.Text = "設定"
    '
    '印刷ToolStripMenuItem
    '
    Me.印刷ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ページ設定ToolStripMenuItem, Me.印刷プレビューToolStripMenuItem, Me.印刷ToolStripMenuItem1})
    Me.印刷ToolStripMenuItem.Name = "印刷ToolStripMenuItem"
    Me.印刷ToolStripMenuItem.Size = New System.Drawing.Size(44, 22)
    Me.印刷ToolStripMenuItem.Text = "印刷"
    '
    'ページ設定ToolStripMenuItem
    '
    Me.ページ設定ToolStripMenuItem.Name = "ページ設定ToolStripMenuItem"
    Me.ページ設定ToolStripMenuItem.Size = New System.Drawing.Size(160, 22)
    Me.ページ設定ToolStripMenuItem.Text = "ページ設定"
    '
    '印刷プレビューToolStripMenuItem
    '
    Me.印刷プレビューToolStripMenuItem.Name = "印刷プレビューToolStripMenuItem"
    Me.印刷プレビューToolStripMenuItem.Size = New System.Drawing.Size(160, 22)
    Me.印刷プレビューToolStripMenuItem.Text = "印刷プレビュー"
    '
    '印刷ToolStripMenuItem1
    '
    Me.印刷ToolStripMenuItem1.Name = "印刷ToolStripMenuItem1"
    Me.印刷ToolStripMenuItem1.Size = New System.Drawing.Size(160, 22)
    Me.印刷ToolStripMenuItem1.Text = "印刷"
    '
    'Label5
    '
    Me.Label5.AutoSize = True
    Me.Label5.Location = New System.Drawing.Point(319, 46)
    Me.Label5.Name = "Label5"
    Me.Label5.Size = New System.Drawing.Size(56, 12)
    Me.Label5.TabIndex = 11
    Me.Label5.Text = "出力コード"
    '
    'cmdCodeRemove
    '
    Me.cmdCodeRemove.Enabled = False
    Me.cmdCodeRemove.Location = New System.Drawing.Point(319, 314)
    Me.cmdCodeRemove.Name = "cmdCodeRemove"
    Me.cmdCodeRemove.Size = New System.Drawing.Size(70, 23)
    Me.cmdCodeRemove.TabIndex = 8
    Me.cmdCodeRemove.Text = "削除"
    Me.cmdCodeRemove.UseVisualStyleBackColor = True
    '
    'cmdCodeClear
    '
    Me.cmdCodeClear.Location = New System.Drawing.Point(395, 314)
    Me.cmdCodeClear.Name = "cmdCodeClear"
    Me.cmdCodeClear.Size = New System.Drawing.Size(70, 23)
    Me.cmdCodeClear.TabIndex = 9
    Me.cmdCodeClear.Text = "クリア"
    Me.cmdCodeClear.UseVisualStyleBackColor = True
    '
    'cmdOutput
    '
    Me.cmdOutput.Location = New System.Drawing.Point(13, 299)
    Me.cmdOutput.Name = "cmdOutput"
    Me.cmdOutput.Size = New System.Drawing.Size(176, 38)
    Me.cmdOutput.TabIndex = 11
    Me.cmdOutput.Text = "出力"
    Me.cmdOutput.UseVisualStyleBackColor = True
    '
    'cmdPreview
    '
    Me.cmdPreview.Location = New System.Drawing.Point(12, 255)
    Me.cmdPreview.Name = "cmdPreview"
    Me.cmdPreview.Size = New System.Drawing.Size(176, 38)
    Me.cmdPreview.TabIndex = 10
    Me.cmdPreview.Text = "プレビュー"
    Me.cmdPreview.UseVisualStyleBackColor = True
    '
    'Label6
    '
    Me.Label6.AutoSize = True
    Me.Label6.Font = New System.Drawing.Font("MS UI Gothic", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
    Me.Label6.Location = New System.Drawing.Point(8, 35)
    Me.Label6.Name = "Label6"
    Me.Label6.Size = New System.Drawing.Size(134, 21)
    Me.Label6.TabIndex = 16
    Me.Label6.Text = "バーコード作成"
    '
    'label7
    '
    Me.label7.AutoSize = True
    Me.label7.Location = New System.Drawing.Point(10, 201)
    Me.label7.Name = "label7"
    Me.label7.Size = New System.Drawing.Size(66, 12)
    Me.label7.TabIndex = 17
    Me.label7.Text = "コードの字数"
    '
    'cboxCodeLength
    '
    Me.cboxCodeLength.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
    Me.cboxCodeLength.FormattingEnabled = True
    Me.cboxCodeLength.Items.AddRange(New Object() {"3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "制限なし"})
    Me.cboxCodeLength.Location = New System.Drawing.Point(82, 195)
    Me.cboxCodeLength.Name = "cboxCodeLength"
    Me.cboxCodeLength.Size = New System.Drawing.Size(121, 24)
    Me.cboxCodeLength.TabIndex = 4
    '
    'lboxOutputCnt
    '
    Me.lboxOutputCnt.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
    Me.lboxOutputCnt.FormattingEnabled = True
    Me.lboxOutputCnt.ItemHeight = 16
    Me.lboxOutputCnt.Location = New System.Drawing.Point(464, 64)
    Me.lboxOutputCnt.Name = "lboxOutputCnt"
    Me.lboxOutputCnt.Size = New System.Drawing.Size(49, 244)
    Me.lboxOutputCnt.TabIndex = 18
    Me.lboxOutputCnt.TabStop = False
    '
    'Label8
    '
    Me.Label8.AutoSize = True
    Me.Label8.Location = New System.Drawing.Point(462, 46)
    Me.Label8.Name = "Label8"
    Me.Label8.Size = New System.Drawing.Size(53, 12)
    Me.Label8.TabIndex = 19
    Me.Label8.Text = "出力件数"
    '
    'PrintDocument1
    '
    '
    'PrintDialog1
    '
    Me.PrintDialog1.UseEXDialog = True
    '
    'MainForm
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(532, 352)
    Me.Controls.Add(Me.Label8)
    Me.Controls.Add(Me.lboxOutputCnt)
    Me.Controls.Add(Me.cboxCodeLength)
    Me.Controls.Add(Me.label7)
    Me.Controls.Add(Me.Label6)
    Me.Controls.Add(Me.cmdPreview)
    Me.Controls.Add(Me.cmdOutput)
    Me.Controls.Add(Me.cmdCodeClear)
    Me.Controls.Add(Me.cmdCodeRemove)
    Me.Controls.Add(Me.Label5)
    Me.Controls.Add(Me.lboxOutputCode)
    Me.Controls.Add(Me.cmdAddCode)
    Me.Controls.Add(Me.Label4)
    Me.Controls.Add(Me.cboxOutputCnt)
    Me.Controls.Add(Me.Label3)
    Me.Controls.Add(Me.Label2)
    Me.Controls.Add(Me.Label1)
    Me.Controls.Add(Me.txtSufix)
    Me.Controls.Add(Me.txtCode)
    Me.Controls.Add(Me.txtPrefix)
    Me.Controls.Add(Me.MenuStrip1)
    Me.MainMenuStrip = Me.MenuStrip1
    Me.Name = "MainForm"
    Me.Text = "バーコード作成"
    Me.MenuStrip1.ResumeLayout(False)
    Me.MenuStrip1.PerformLayout()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub

  Friend WithEvents txtPrefix As TextBox
  Friend WithEvents txtCode As TextBox
  Friend WithEvents txtSufix As TextBox
  Friend WithEvents Label1 As Label
  Friend WithEvents Label2 As Label
  Friend WithEvents Label3 As Label
  Friend WithEvents cboxOutputCnt As ComboBox
  Friend WithEvents Label4 As Label
  Friend WithEvents cmdAddCode As Button
  Friend WithEvents lboxOutputCode As ListBox
  Friend WithEvents MenuStrip1 As MenuStrip
  Friend WithEvents Label5 As Label
  Friend WithEvents cmdCodeRemove As Button
  Friend WithEvents cmdCodeClear As Button
  Friend WithEvents cmdOutput As Button
  Friend WithEvents cmdPreview As Button
  Friend WithEvents 設定ToolStripMenuItem As ToolStripMenuItem
  Friend WithEvents Label6 As Label
  Friend WithEvents label7 As Label
  Friend WithEvents cboxCodeLength As ComboBox
  Friend WithEvents lboxOutputCnt As ListBox
  Friend WithEvents Label8 As Label
  Friend WithEvents 印刷ToolStripMenuItem As ToolStripMenuItem
  Friend WithEvents ページ設定ToolStripMenuItem As ToolStripMenuItem
  Friend WithEvents 印刷プレビューToolStripMenuItem As ToolStripMenuItem
  Friend WithEvents 印刷ToolStripMenuItem1 As ToolStripMenuItem
  Friend WithEvents PrintDocument1 As Printing.PrintDocument
  Friend WithEvents PrintDialog1 As PrintDialog
End Class
