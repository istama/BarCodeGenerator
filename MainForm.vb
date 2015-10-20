
Imports System.IO
Imports System.Drawing.Printing
Imports MP.Details.IO
Imports MP.Utils.Common
Imports MP.BarCodeGenerator

Public Class MainForm
  Private Setting As AppProperties.Entry = AppProperties.SETTING

  Private LBoxOfCodeAndCnt As ListBoxOfCodeAndCnt

  Private BarCode As Print.BarCode
  Private PageSetting As New PageSettings

  Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    Try
      AutoUpdate()
    Catch ex As Exception
    End Try

    txtCode.Focus()
    LBoxOfCodeAndCnt = New ListBoxOfCodeAndCnt(lboxOutputCode, lboxOutputCnt)
    BarCode = New Print.BarCode()

    PageSetting = PrintDocument1.DefaultPageSettings
  End Sub

  Private Sub AutoUpdate()
    Dim versionFilePath = Setting.GetValue(AppProperties.KEY_RELEASE_VERSIONINFO_FILE_DIR) & "\" & Setting.GetValue(AppProperties.KEY_RELEASE_VERSIONINFO_FILE_NAME)
    Dim text As List(Of String) = FileAccessor.Read(versionFilePath)
    If text.Count > 0 AndAlso Not Version.IsLatestVersion(text(0)) Then
      MessageBox.Show("最新のバージョンに更新します。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Information)
      System.Diagnostics.Process.Start(FilePath.ScriptForUpdatePath())
      MessageBox.Show("更新終了しました。再起動して下さい。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Information)
      Me.Close()
    End If
  End Sub

  Private Sub TextBox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCode.KeyPress, txtPrefix.KeyPress, txtSufix.KeyPress
    ' case Pressed Enter Key
    If e.KeyChar = Chr(13) Then
      AddCodeToOutputList()
    ElseIf Not BCFont.NW7.IsCharCodeValid(e.KeyChar) AndAlso e.KeyChar <> vbBack Then
      e.Handled = True
    End If
  End Sub

  Private Sub ComboBox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cboxOutputCnt.KeyPress, cboxCodeLength.KeyPress
    ' case Pressed Enter Key
    If e.KeyChar = Chr(13) Then
      AddCodeToOutputList()
    ElseIf Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> vbBack Then
      e.Handled = True
    End If
  End Sub

  Private Sub cmdAddCode_Click(sender As Object, e As EventArgs) Handles cmdAddCode.Click
    AddCodeToOutputList()
  End Sub

  Private Sub AddCodeToOutputList()
    Dim prefix As String = txtPrefix.Text
    Dim code As String = txtCode.Text
    Dim sufix As String = txtSufix.Text
    Dim cnt As String = cboxOutputCnt.SelectedItem

    Dim output As String = Padding0ToOutputCode(prefix, code, sufix)
    If output.Length = 0 Then
      MessageBox.Show("コードが入力されていません。")
    ElseIf Not Char.IsDigit(cnt) OrElse cnt <= 0 Then
      MessageBox.Show("出力件数が不正です。")
    ElseIf Not IsCodeLengthValid(output) Then
      MessageBox.Show("コードの字数が間違っています。" & vbCrLf & output)
    ElseIf Not BCFont.NW7.IsStrCodeValide(output) Then
      MessageBox.Show("コードが不正です。" & vbCrLf & output)
    Else
      LBoxOfCodeAndCnt.Add(output, Integer.Parse(cnt))
      txtCode.Text = ""
      UpdateLabelOfCodeAndOutputCnt()
      cmdOutput.Enabled = False
      txtCode.Focus()
    End If
  End Sub

  Private Function IsCodeLengthValid(code As String) As Boolean
    Dim item As String = cboxCodeLength.SelectedItem
    Return Not Char.IsDigit(item) OrElse code.Length = Integer.Parse(item)
  End Function

  Private Function Padding0ToOutputCode(prefix As String, code As String, sufix As String) As String
    Dim joined As String = prefix & code & sufix

    If Not chk0Padd.Checked OrElse Not Char.IsDigit(cboxCodeLength.SelectedItem) Then
      Return joined
    Else
      Dim length As Integer = Integer.Parse(cboxCodeLength.SelectedItem)
      Dim padcnt As Integer = length - joined.Length

      If padcnt > 0 AndAlso chk0Padd.Checked = True Then
        Dim zero As String = New String("0"c, padcnt)

        Dim loc As String = cbox0PadLocation.SelectedItem
        If loc = "接頭辞の前" Then
          Return zero & joined
        ElseIf loc = "接頭辞の後" Then
          Return prefix & zero & code & sufix
        ElseIf loc = "接尾辞の前" Then
          Return prefix & code & zero & sufix
        ElseIf loc = "接尾辞の後" Then
          Return joined & zero
        Else
          Return joined
        End If
      Else
        Return joined
      End If
    End If
  End Function

  Private Sub lboxOutputCode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lboxOutputCode.SelectedIndexChanged
    If LBoxOfCodeAndCnt.CntSelectedItems > 0 Then
      cmdCodeRemove.Enabled = True
    Else
      cmdCodeRemove.Enabled = False
    End If
  End Sub

  Private Sub cmdCodeRemove_Click(sender As Object, e As EventArgs) Handles cmdCodeRemove.Click
    LBoxOfCodeAndCnt.RemoveSelectedItems()
    UpdateLabelOfCodeAndOutputCnt()
  End Sub

  Private Sub cmdCodeClear_Click(sender As Object, e As EventArgs) Handles cmdCodeClear.Click
    If MessageBox.Show("クリアしてよろしいですか？", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) = DialogResult.OK Then
      ClearOutputCodeList()
    End If
  End Sub

  Private Sub ClearOutputCodeList()
    LBoxOfCodeAndCnt.Clear()
    UpdateLabelOfCodeAndOutputCnt()
  End Sub

  Private Sub UpdateLabelOfCodeAndOutputCnt()
    lblCodeCnt.Text = LBoxOfCodeAndCnt.CntCode()
    lblOutputCnt.Text = LBoxOfCodeAndCnt.CntAll()
  End Sub

  Private Sub lboxOutputCnt_DoubleCkick(sender As Object, e As EventArgs) Handles lboxOutputCnt.DoubleClick
    'MessageBox.Show(sender.ToString & " " & lboxOutputCnt.SelectedIndex)
  End Sub

  Private Sub Draw()
    If LBoxOfCodeAndCnt.CntCode > 0 Then
      BarCode.Draw(LBoxOfCodeAndCnt.OutputCodeList())
    Else
      Throw New Exception("コードが入力されていません。")
    End If
  End Sub

  Private Sub cmdPreview_Click(sender As Object, e As EventArgs) Handles cmdPreview.Click
    Try
      ShowPrintPreview()
      cmdOutput.Enabled = True
    Catch ex As Exception
      MsgBox.ShowWarn(ex)
    End Try
  End Sub

  Private Sub cmdOutput_Click(sender As Object, e As EventArgs) Handles cmdOutput.Click
    Try
      ShowPrintDialog()
    Catch ex As Exception
      MsgBox.ShowWarn(ex)
    End Try
  End Sub

  Private Sub ShowPrintDialog()
    'PrintDocument
    '  プリンタの設定（PrinterSettingsクラス）や印刷の設定（PageSettingsクラス）を行い
    '  Print()で印刷処理の一連のプロセスを開始する。
    '  このメソッドで、PrintDocumentクラスで定義されているPrintPageイベントを発生させ、
    '  このイベントに連動して呼び出されるイベントハンドラの内容に基づいて印刷を行う。
    '  なので、印刷内容はこのPrintPageイベントハンドラに記述する。
    'PrintDialog
    '  出力先のプリンタや印刷範囲などの印刷設定を行う印刷ダイアログボックスを開く。
    'PrintPreviewDialog
    '  印刷プレビューを開く。
    '  開いたときにPrintPageイベントハンドラが呼び出される。
    'PrintPreviewControl
    '  印刷プレビューのフォームを自分で作るためのコントロール。
    '  開いたときにPrintPageイベントハンドラが呼び出される。
    'PageSetupDialog
    '  印刷ページの設定ダイアログを開く。

    'PageSetting = PrintDocument1.DefaultPageSettings
    'DocPrint = CaptureControlPS(BarCodeForm.TableLayoutPanel1)
    PrintDialog1.Document = PrintDocument1

    'プリンタの選択や印刷設定を行うダイアログを表示する
    If PrintDialog1.ShowDialog() = DialogResult.OK Then
      PrintDocument1.Print()
      ClearOutputCodeList()
      txtCode.Focus()
      cmdOutput.Enabled = False
    End If
  End Sub

  'Private Sub ページ設定ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ページ設定ToolStripMenuItem.Click
  '  PageSetupDialog1.PageSettings = PageSetting
  '  PageSetupDialog1.ShowDialog()
  'End Sub

  Private Sub ShowPrintPreview()
    PrintDocument1.DefaultPageSettings = PageSetting
    'PrintPreviewDialog1.Document = PrintDocument1
    'PrintPreviewDialog1.ShowDialog()
    PreviewForm.PrintPreviewControl1.Document = PrintDocument1
    PreviewForm.PrintPreviewControl1.Show()
    PreviewForm.ShowDialog()
  End Sub

  Private Sub PrintDocument1_PrintPage(sender As Object, e As PrintPageEventArgs) Handles PrintDocument1.PrintPage
    'PrintDocument.Print()を実行すると呼び出される。
    'どのような印刷を行うのかをここで指定する。
    'MessageBox.Show("PrintDocument1_PrintPage")
    Try
      Draw()
      Dim bmp As Bitmap = BarCode.NextPrintPage()
      e.Graphics.DrawImage(bmp, 0, 0)
      If BarCode.HasNextPrintPage() Then
        e.HasMorePages = True
      Else
        e.HasMorePages = False
        BarCode.ToStartPrintPage()
        PreviewForm.PrintPreviewControl1.Rows = BarCode.CntPrintPage()
      End If
    Catch ex As Exception
      MsgBox.ShowWarn(ex)
    End Try
  End Sub

End Class

Class ListBoxOfCodeAndCnt
  Private LBoxCode As ListBox
  Private LBoxCnt As ListBox

  Sub New(lboxCode As ListBox, lboxCnt As ListBox)
    Me.LBoxCode = lboxCode
    Me.LBoxCnt = lboxCnt
  End Sub

  Public Sub Add(code As String, cnt As Integer)
    LBoxCode.Items.Add(code)
    LBoxCnt.Items.Add(cnt.ToString)
  End Sub

  Public Sub RemoveSelectedItems()
    If LBoxCode.SelectedIndex >= 0 Then
      Dim idx = LBoxCode.SelectedIndex
      RemoveAt(idx)
      If LBoxCode.SelectedIndex >= 0 Then
        RemoveSelectedItems()
      End If
    End If
  End Sub

  Public Sub RemoveAt(idx As Integer)
    If idx >= 0 AndAlso idx < LBoxCode.Items.Count Then
      LBoxCode.Items.RemoveAt(idx)
      LBoxCnt.Items.RemoveAt(idx)
    End If
  End Sub

  Public Sub Clear()
    LBoxCode.Items.Clear()
    LBoxCnt.Items.Clear()
  End Sub

  Public Function CntSelectedItems() As Integer
    Return LBoxCode.SelectedItems.Count
  End Function

  Public Function CntCode() As Integer
    Return LBoxCode.Items.Count()
  End Function

  Public Function CntAll() As Integer
    Dim total As Integer = 0
    For Each cnt As String In LBoxCnt.Items
      If Char.IsDigit(cnt) Then
        total += Integer.Parse(cnt)
      End If
    Next
    Return total
  End Function

  Public Function OutputCodeList() As List(Of String)
    Dim codeList As New List(Of String)
    For i As Integer = 0 To (LBoxCode.Items.Count - 1)
      If Char.IsDigit(LBoxCnt.Items(i)) Then
        For j As Integer = 1 To Integer.Parse(LBoxCnt.Items(i))
          codeList.Add(LBoxCode.Items(i))
        Next
      End If
    Next
    Return codeList
  End Function
End Class