Public Class Drawer
  Private PictureBox As PictureBox

  Public Sub New(pictureBox As PictureBox)
    Me.PictureBox = pictureBox
  End Sub

  Public Function Draw(code As String) As PictureBox
    '描画先とするImageオブジェクトを作成する
    Dim canvas As New Bitmap(PictureBox.Width, PictureBox.Height)
    'ImageオブジェクトのGraphicsオブジェクトを作成する
    Dim g As Graphics = Graphics.FromImage(canvas)

    Dim bcFont As Font = GetNW7Font(12)
    Dim iFont As New Font("MS Gothic", 12)
    '文字列を位置(0,0)、青色で表示
    g.DrawString(code, bcFont, Brushes.Black, 0, 0)
    'g.DrawString(code, bcFont, Brushes.Black, 0, 20)
    g.DrawString(code, iFont, Brushes.Black, 0, 40)

    'リソースを解放する
    bcFont.Dispose()
    iFont.Dispose()
    g.Dispose()

    'PictureBox1に表示する
    PictureBox.Image = canvas

    Return PictureBox
  End Function

  Public Function GetNW7Font(size As Integer) As Font
    'PrivateFontCollectionオブジェクトを作成する
    Dim pfc As New System.Drawing.Text.PrivateFontCollection()
    'PrivateFontCollectionにフォントを追加する
    pfc.AddFontFile("C:\Users\Blue\Documents\sourcecode\visualbasic\NW-7.ttf")

    Dim NW7 As FontFamily = Nothing
    For Each ff As System.Drawing.FontFamily In pfc.Families
      If ff.Name = "NW-7" Then
        NW7 = ff
        Exit For
      End If
    Next

    If NW7 Is Nothing Then
      Throw New Exception("NW-7が見つかりません。")
    End If

    'フォントオブジェクトの作成
    Return New Font(NW7, size, FontStyle.Regular)
  End Function
End Class
