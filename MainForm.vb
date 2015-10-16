
Imports System.IO
Imports System.Drawing.Printing
Imports MP.Utils.Common
Imports MP.BarCodeGenerator

Public Class MainForm

  Private LBoxOfCodeAndCnt As ListBoxOfCodeAndCnt

  Private DocPrint As Bitmap
  Private PageSetting As New PageSettings

  Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    txtCode.Focus()
    LBoxOfCodeAndCnt = New ListBoxOfCodeAndCnt(lboxOutputCode, lboxOutputCnt)
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

    Dim output As String = prefix & code & sufix
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
      txtCode.Focus()
    End If
  End Sub

  Private Function IsCodeLengthValid(code As String) As Boolean
    Dim item As String = cboxCodeLength.SelectedItem
    Return Not Char.IsDigit(item) OrElse code.Length = Integer.Parse(item)
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
  End Sub

  Private Sub cmdCodeClear_Click(sender As Object, e As EventArgs) Handles cmdCodeClear.Click
    LBoxOfCodeAndCnt.Clear()
  End Sub

  Private Sub lboxOutputCnt_DoubleCkick(sender As Object, e As EventArgs) Handles lboxOutputCnt.DoubleClick
    MessageBox.Show(sender.ToString & " " & lboxOutputCnt.SelectedIndex)

  End Sub

  Private Sub cmdPreview_Click(sender As Object, e As EventArgs) Handles cmdPreview.Click
    Draw()
    PreviewForm.ShowDialog()
  End Sub

  Private Sub Draw()
    PreviewForm.TableLayoutPanel1.Controls.Clear()

    Try
      Dim col As Integer = PreviewForm.TableLayoutPanel1.ColumnCount
      Dim barCodeDrawer As Graphic.BarCodeDrawer = New Graphic.BarCodeDrawer(BCFont.NW7.CreateFont(14), New Font("MS UI Gothic", 11, FontStyle.Regular))

      If LBoxOfCodeAndCnt.CntCode() > 0 Then
        Dim codeList As List(Of String) = LBoxOfCodeAndCnt.OutputCodeList()

        For idx As Integer = 0 To (codeList.Count - 1)
          Dim r As Integer = Math.Truncate(idx / col)
          Dim c As Integer = idx Mod col

          If r < PreviewForm.TableLayoutPanel1.RowCount Then
            Dim panel As Panel = barCodeDrawer.Create(codeList(idx))
            PreviewForm.TableLayoutPanel1.Controls.Add(panel, c, r)
          End If

        Next
      End If
    Catch ex As Exception
      MsgBox.ShowError(ex)
    End Try
  End Sub

  Private Sub cmdOutput_Click(sender As Object, e As EventArgs) Handles cmdOutput.Click
    Draw()

    PreviewForm.Show()
    Dim ctrl As Control = PreviewForm.TableLayoutPanel1

    'プリントスクリーンによるキャプチャ
    Dim bmp3 As Bitmap = CaptureControlPS(ctrl)
    bmp3.Save("C:\Users\Blue\3.png")
    bmp3.Dispose()

    PreviewForm.Close()
  End Sub

  <System.Runtime.InteropServices.DllImport("User32.dll")>
  Private Shared Function PrintWindow(ByVal hwnd As IntPtr,
    ByVal hDC As IntPtr, ByVal nFlags As Integer) As Boolean
  End Function

  ''' <summary>
  ''' コントロールのイメージを取得する
  ''' </summary>
  ''' <param name="ctrl">キャプチャするコントロール</param>
  ''' <returns>取得できたイメージ</returns>
  Public Function CaptureControlPS(ByVal ctrl As Control) As Bitmap
    Dim img As New Bitmap(ctrl.Width, ctrl.Height)
    Dim memg As Graphics = Graphics.FromImage(img)
    Dim dc As IntPtr = memg.GetHdc()
    PrintWindow(ctrl.Handle, dc, 0)
    memg.ReleaseHdc(dc)
    memg.Dispose()
    Return img
  End Function

  Private Sub 印刷ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles 印刷ToolStripMenuItem1.Click
    'PrintDocument
    '  プリンタの設定（PrinterSettingsクラス）や印刷の設定（PageSettingsクラス）を行い
    '  Print()で印刷処理の一連のプロセスを開始する。
    '  このメソッドで、PrintDocumentクラスで定義されているPrintPageイベントを発生させ、
    '  このイベントに連動して呼び出されるイベントハンドラの内容に基づいて印刷を行う。
    '  なので、印刷内容はこのPrintPageイベントハンドラに記述する。
    'PrintDialog
    '  出力先のプリンタや印刷範囲などの印刷設定を行う印刷ダイアログボックスを開く。


    Try
      PageSetting = PrintDocument1.DefaultPageSettings
      'DocPrint = CaptureControlPS(PreviewForm.TableLayoutPanel1)
      PrintDialog1.Document = PrintDocument1

      'プリンタの選択や印刷設定を行うダイアログを表示する
      If PrintDialog1.ShowDialog() = DialogResult.OK Then
        PrintDocument1.Print()
      End If
    Catch ex As Exception
      MsgBox.ShowWarn(ex)
    End Try
  End Sub

  Private Sub 印刷プレビューToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 印刷プレビューToolStripMenuItem.Click


  End Sub

  Private Sub PrintDocument1_PrintPage(sender As Object, e As PrintPageEventArgs) Handles PrintDocument1.PrintPage
    'PrintDocument.Print()を実行すると呼び出される。
    'どのような印刷を行うのかをここで指定する。
    'MessageBox.Show("PrintDocument1_PrintPage")

    Draw()
    PreviewForm.Show()
    e.Graphics.DrawImage(CaptureControlPS(PreviewForm.TableLayoutPanel1), 0, 0)
    PreviewForm.Close()
    e.HasMorePages = False
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