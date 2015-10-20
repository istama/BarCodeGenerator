
Imports MP.Utils.Common

Namespace BarCodeGenerator

  Namespace BCFont
    Public Module NW7
      Private NumberCharList As New List(Of Char) From {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"}
      Private AlphCharList As New List(Of Char) From {"A", "B", "C", "D"}
      Private SignCharList As New List(Of Char) From {"-", "$", ":", "/", ".", "+"}
      Private CharList As List(Of Char) = UnifyList()

      Public Function IsCharCodeValid(code As Char) As Boolean
        Return CharList.Contains(code)
      End Function

      Public Function IsStrCodeValide(code As String) As Boolean
        If Not AlphCharList.Contains(code.First) OrElse Not AlphCharList.Contains(code.Last) Then
          Return False
        End If

        Dim valid = True

        For Each c As Char In code.ToCharArray()
          If Not IsCharCodeValid(c) Then
            valid = False
            Exit For
          End If
        Next
        Return valid
      End Function

      Private Function UnifyList() As List(Of Char)
        Dim l As New List(Of Char)(NumberCharList)
        l.AddRange(AlphCharList)
        l.AddRange(SignCharList)
        Return l
      End Function

      Public Function CreateFont(size As Integer) As Font
        'PrivateFontCollectionオブジェクトを作成する
        Dim pfc As New System.Drawing.Text.PrivateFontCollection()
        'PrivateFontCollectionにフォントを追加する
        Dim fontFilePath As String = FilePath.GetPath(AppProperties.SETTING, AppProperties.KEY_NW7_FILE_DIR, AppProperties.KEY_NW7_FILE_NAME)
        pfc.AddFontFile(fontFilePath)

        Dim NW7ff As FontFamily = Nothing
        For Each ff As System.Drawing.FontFamily In pfc.Families
          If ff.Name = "NW-7" Then
            NW7ff = ff
            Exit For
          End If
        Next

        If NW7ff IsNot Nothing Then
          'フォントオブジェクトの作成
          Return New Font(NW7ff, size, FontStyle.Regular)
        Else
          Throw New Exception("NW-7が見つかりません。")
        End If
      End Function
    End Module

  End Namespace

  Namespace Print

    Public Class BarCode
      Private Const BARCODE_FONT_SIZE = 36
      Private Const CODE_FONT_SIZE = 30
      Private Const ROW_IN_SHEET As Integer = 15
      Private Const COL_IN_SHEET As Integer = 4
      Private Const DPI_RESOLUTION As Integer = 230

      Private Arrange As BarCodeArrange

      Private BitmapList As New List(Of Bitmap)
      Private CodeListCache As New List(Of String)

      Private CurrentPage As Integer

      Sub New()
        Arrange = New Print.BarCodeArrange(BarCodeForm.TableLayoutPanel1, ROW_IN_SHEET, COL_IN_SHEET)
        CurrentPage = 0
      End Sub

      Public Sub Draw(codeList As List(Of String))
        If Not EqualCache(codeList) Then
          BitmapList.Clear()
          Create(codeList)
          CodeListCache = New List(Of String)(codeList)
        End If
      End Sub

      Private Function EqualCache(list As List(Of String)) As Boolean
        If list.Count = CodeListCache.Count Then
          Dim equal As Boolean = True
          For i As Integer = 0 To (list.Count - 1)
            If list(i) <> CodeListCache(i) Then
              Exit For
            End If
          Next
          Return equal
        Else
          Return False
        End If
      End Function

      Private Sub Create(codeList As List(Of String))
        If codeList.Count > 0 Then
          Dim bmpList As New List(Of Bitmap)

          Dim blockCnt As Integer = Arrange.CreatedBlockCnt(codeList.Count)
          If blockCnt > Arrange.CntBlockInAPrintPage() Then
            blockCnt = Arrange.CntBlockInAPrintPage()
          End If

          Try
            For bIdx As Integer = 0 To (blockCnt - 1)
              BarCodeForm.TableLayoutPanel1.Controls.Clear()

              Dim barCodeDrawer As Print.BarCodeDrawer = New Print.BarCodeDrawer(BCFont.NW7.CreateFont(BARCODE_FONT_SIZE), New Font("MS UI Gothic", CODE_FONT_SIZE, FontStyle.Regular))

              'ブロックの左上のコードのインデックスを求める
              Dim offset As Integer = Arrange.ToCellIdxFrom(bIdx)

              For i As Integer = 0 To (Arrange.BlockCellCnt() - 1)
                Dim c As Integer = i Mod Arrange.ColInBlock()
                Dim r As Integer = Math.Truncate(i / Arrange.ColInBlock())

                Dim idx As Integer = offset + (r * COL_IN_SHEET) + c
                If idx < codeList.Count AndAlso idx < Arrange.CntCellInAPrintPage() Then
                  Dim panel As Panel = barCodeDrawer.Create(codeList(idx))
                  BarCodeForm.TableLayoutPanel1.Controls.Add(panel, c, r)
                End If
              Next

              Dim bmp As Bitmap = CreateBitmapOfBarCodeForm()
              bmpList.Add(bmp)
            Next
          Catch ex As Exception
            If MessageBox.Show("バーコードの作成に失敗しました。", "エラー", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) = DialogResult.Retry Then
              Create(codeList)
              Return
            Else
              Throw ex
            End If
          End Try

          BitmapList.AddRange(bmpList)

          If Arrange.HasNextPage(codeList.Count()) Then
            Create(New List(Of String)(codeList.Skip(Arrange.CntCellInAPrintPage())))
          End If

        Else
          Throw New Exception("コードがありません。")
        End If
      End Sub

      Public Sub ToStartPrintPage()
        If BitmapList.Count() > 0 Then
          CurrentPage = 0
        Else
          Throw New Exception("Bitmapが生成されていません。")
        End If
      End Sub

      Public Function NextPrintPage() As Bitmap
        If BitmapList.Count() = 0 Then
          Throw New Exception("Bitmapが生成されていません。")
        ElseIf Not HasNextPrintPage() Then
          Throw New Exception("ページが終端に達しました。")
        Else
          Dim aPageBmps As List(Of Bitmap) = GetBitmapListOfAPage(CurrentPage)
          Dim bmp As Bitmap = CombineBitmap(aPageBmps)
          CurrentPage += 1
          Return bmp
        End If
      End Function

      Public Function HasNextPrintPage() As Boolean
        If BitmapList.Count() = 0 Then
          Throw New Exception("Bitmapが生成されていません。")
        Else
          Return BitmapList.Count() > Arrange.CntBlockInAPrintPage * CurrentPage
        End If
      End Function

      Public Function CntPrintPage() As Integer
        If BitmapList.Count() = 0 Then
          Throw New Exception("Bitmapが生成されていません。")
        Else
          Return Math.Ceiling(BitmapList.Count() / Arrange.CntBlockInAPrintPage())
        End If
      End Function

      Private Function GetBitmapListOfAPage(page As Integer) As List(Of Bitmap)
        Dim skiped As IEnumerable(Of Bitmap) = BitmapList.Skip(Arrange.CntBlockInAPrintPage() * page)
        Dim taken As IEnumerable(Of Bitmap) = New List(Of Bitmap)(skiped).Take(Arrange.CntBlockInAPrintPage())
        Return New List(Of Bitmap)(taken)
      End Function

      Private Function CombineBitmap(bmpList As List(Of Bitmap)) As Bitmap
        If bmpList.Count > 0 Then
          Dim bmp As Bitmap = bmpList(0)

          '※ビットマップのサイズはコピー元と先でぴったり合わせないと印刷時に色がおかしくなる
          Dim width As Integer = bmp.Width * Arrange.BlockColInSheet()
          Dim height As Integer = bmp.Height * Math.Ceiling(bmpList.Count() / Arrange.BlockColInSheet())
          Dim bmpNew As Bitmap = New Bitmap(width, height, Imaging.PixelFormat.Format32bppArgb)

          ' 新しいBitmapオブジェクトに元の画像内容を描画
          Dim g As Graphics = Graphics.FromImage(bmpNew)
          For idx As Integer = 0 To (bmpList.Count - 1)
            Dim left As Integer = (idx Mod Arrange.BlockColInSheet()) * bmp.Width
            Dim Top As Integer = bmp.Height * Math.Truncate(idx / Arrange.BlockColInSheet())
            g.DrawImage(bmpList(idx), left, Top, bmp.Width, bmp.Height)
          Next
          g.Dispose()

          Return ConvertResolution(bmpNew, DPI_RESOLUTION)
        Else
          Throw New Exception("ビットマップリストが空です。")
        End If
      End Function

      Private Function ConvertResolution(bmp As Bitmap, resolution As Double) As Bitmap
        ' 画像解像度を変更して新しいBitmapオブジェクトを作成
        Dim bmpNew As Bitmap = New Bitmap(bmp.Width, bmp.Height, Imaging.PixelFormat.Format32bppArgb)
        bmpNew.SetResolution(resolution, resolution)

        ' 新しいBitmapオブジェクトに元の画像内容を描画
        Dim g As Graphics = Graphics.FromImage(bmpNew)
        g.DrawImage(bmp, 0, 0, bmp.Width, bmp.Height)
        g.Dispose()

        Return bmpNew
      End Function

      Private Function CreateBitmapOfBarCodeForm() As Bitmap
        BarCodeForm.Show()
        Dim bmp As Bitmap = CaptureControlPS(BarCodeForm.TableLayoutPanel1)
        BarCodeForm.Close()
        Return bmp
      End Function

      'Win32APIのコマンドを呼び出している
      <System.Runtime.InteropServices.DllImport("User32.dll")>
      Private Shared Function PrintWindow(ByVal hwnd As IntPtr, ByVal hDC As IntPtr, ByVal nFlags As Integer) As Boolean
      End Function

      ''' <summary>
      ''' コントロールのイメージを取得する
      ''' </summary>
      ''' <param name="ctrl">キャプチャするコントロール</param>
      ''' <returns>取得できたイメージ</returns>
      Public Function CaptureControlPS(ByVal ctrl As Control) As Bitmap
        Dim img As New Bitmap(ctrl.Width, ctrl.Height, Imaging.PixelFormat.Format32bppArgb)
        Dim memg As Graphics = Graphics.FromImage(img)
        Dim dc As IntPtr = memg.GetHdc()
        PrintWindow(ctrl.Handle, dc, 0)
        memg.ReleaseHdc(dc)
        memg.Dispose()
        Return img
      End Function

      Public Function Compress(bmp As Bitmap, proportion As Double) As Bitmap
        If proportion <= 0.0 OrElse proportion > 1.0 Then
          Throw New Exception("比率の指定が間違っています / " & proportion)
        End If

        Dim width As Integer = Math.Truncate(bmp.Width * proportion)
        Dim height As Integer = Math.Truncate(bmp.Height * proportion)

        '描画先とするImageオブジェクトを作成する
        Dim canvas As New Bitmap(width, height)
        canvas.SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution)
        'ImageオブジェクトのGraphicsオブジェクトを作成する
        Dim g As Graphics = Graphics.FromImage(canvas)

        ''補間方法として最近傍補間を指定する
        ''g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor
        '補間方法として高品質双三次補間を指定する
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic
        '画像を縮小して描画する
        g.DrawImage(bmp, 0, 0, width, height)

        Return canvas
      End Function

      'Public Function CaptureControlCS(ByVal ctrl As Control) As Bitmap
      '  Dim rc As Rectangle = BarCodeForm.RectangleToScreen(ctrl.Bounds)

      '  ' Bitmapオブジェクトにスクリーン・キャプチャ
      '  Dim bmp As New Bitmap(rc.Width, rc.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb)
      '  Using g As Graphics = Graphics.FromImage(bmp)

      '    g.CopyFromScreen(rc.X, rc.Y, 0, 0, rc.Size, CopyPixelOperation.SourceCopy)

      '    ' ビット・ブロック転送方式の切り替え例：
      '    'g.FillRectangle(Brushes.LightPink, _
      '    '  0, 0, rc.Width, rc.Height)
      '    'g.CopyFromScreen(rc.X, rc.Y, 0, 0, _
      '    '  rc.Size, CopyPixelOperation.SourceAnd)

      '  End Using

      '  Return bmp
      'End Function

      'Public Shared Function AdjustContrast(ByVal img As Image, ByVal contrast As Single) As Image
      '  'コントラストを変更した画像の描画先となるImageオブジェクトを作成
      '  Dim newImg As New Bitmap(img.Width, img.Height)
      '  'newImgのGraphicsオブジェクトを取得
      '  Dim g As Graphics = Graphics.FromImage(newImg)

      '  'ColorMatrixオブジェクトの作成
      '  Dim scale As Single = (100.0F + contrast) / 100.0F
      '  scale *= scale
      '  Dim append As Single = 0.5F * (1.0F - scale)
      '  Dim cm As New System.Drawing.Imaging.ColorMatrix(New Single()() _
      '      {New Single() {scale, 0, 0, 0, 0},
      '       New Single() {0, scale, 0, 0, 0},
      '       New Single() {0, 0, scale, 0, 0},
      '       New Single() {0, 0, 0, 1, 0},
      '       New Single() {append, append, append, 0, 1}})

      '  'ImageAttributesオブジェクトの作成
      '  Dim ia As New System.Drawing.Imaging.ImageAttributes()
      '  'ColorMatrixを設定する
      '  ia.SetColorMatrix(cm)

      '  'ImageAttributesを使用して描画
      '  g.DrawImage(img, New Rectangle(0, 0, img.Width, img.Height),
      '              0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia)

      '  'リソースを解放する
      '  g.Dispose()

      '  Return newImg
      'End Function

      'Public Shared Function Create1bppImage(ByVal img As Bitmap, resolution As Integer) As Bitmap
      '  '1bppイメージを作成する
      '  Dim newImg As New Bitmap(img.Width, img.Height,
      '                       System.Drawing.Imaging.PixelFormat.Format1bppIndexed)
      '  newImg.SetResolution(resolution, resolution)

      '  'Bitmapをロックする
      '  Dim bmpDate As System.Drawing.Imaging.BitmapData = newImg.LockBits(
      '    New Rectangle(0, 0, newImg.Width, newImg.Height),
      '    System.Drawing.Imaging.ImageLockMode.WriteOnly, newImg.PixelFormat)

      '  '新しい画像のピクセルデータを作成する
      '  Dim pixels As Byte() = New Byte(bmpDate.Stride * bmpDate.Height - 1) {}
      '  For y As Integer = 0 To bmpDate.Height - 1
      '    For x As Integer = 0 To bmpDate.Width - 1
      '      '明るさが0.5以上の時は白くする
      '      If 0.5F <= img.GetPixel(x, y).GetBrightness() Then
      '        'ピクセルデータの位置
      '        Dim pos As Integer = (x >> 3) + bmpDate.Stride * y
      '        '白くする
      '        pixels(pos) = pixels(pos) Or CByte(&H80 >> (x And &H7))
      '      End If
      '    Next
      '  Next
      '  '作成したピクセルデータをコピーする
      '  Dim ptr As IntPtr = bmpDate.Scan0
      '  System.Runtime.InteropServices.Marshal.Copy(pixels, 0, ptr, pixels.Length)

      '  'ロックを解除する
      '  newImg.UnlockBits(bmpDate)

      '  Return newImg
      'End Function

    End Class

    Public Class BarCodeArrange
      Private OneBlockTable As TableLayoutPanel
      Private RowInSheet As Integer
      Private ColInSheet As Integer

      Sub New(oneBlockTable As TableLayoutPanel, rowInSHeet As Integer, colInSheet As Integer)
        Me.OneBlockTable = oneBlockTable
        Me.RowInSheet = rowInSHeet
        Me.ColInSheet = colInSheet
      End Sub

      Public Function RowInBlock() As Integer
        Return OneBlockTable.RowCount
      End Function

      Public Function ColInBlock() As Integer
        Return OneBlockTable.ColumnCount
      End Function

      Public Function BlockCellCnt() As Integer
        Return RowInBlock() * ColInBlock()
      End Function

      Public Function CreatedBlockCnt(cntCode As Integer) As Integer
        If cntCode <= CntCellInAPrintPage() Then
          Dim row As Integer = Math.Ceiling(cntCode / (BlockCellCnt() * BlockColInSheet()))
          Return row * BlockColInSheet()
        Else
          Return Math.Ceiling(RowInSheet / RowInBlock()) * BlockColInSheet()
        End If
      End Function

      Public Function BlockColInSheet() As Integer
        Return Math.Truncate(ColInSheet / ColInBlock())
      End Function

      'ブロックのインデックスをセルのインデックスに変換する
      Public Function ToCellIdxFrom(blockIdx As Integer) As Integer
        '何列目のブロックかを求める
        Dim colIdx As Integer = blockIdx Mod ColInBlock()
        'ブロックの左上のコードのインデックスを求める
        Return BlockCellCnt() * (blockIdx - colIdx) + (ColInBlock() * colIdx)
      End Function

      Public Function CntBlockInAPrintPage() As Integer
        Return Math.Ceiling(RowInSheet / RowInBlock()) * ColInBlock()
      End Function

      Public Function CntCellInAPrintPage() As Integer
        Return RowInSheet * ColInSheet
      End Function

      Public Function CntPrintPage(cntCode As Integer) As Integer
        Return Math.Ceiling(cntCode / CntCellInAPrintPage())
      End Function

      Public Function HasNextPage(cntCode As Integer) As Boolean
        Return cntCode > CntCellInAPrintPage()
      End Function

    End Class

    Public Class BarCodeDrawer
      Private BarCodeFont As Font
      Private CodeFont As Font

      Private PanelFrame As Frame = CreatePanelFrame()
      Private BarCodeFrame1 As Frame = CreateBarCodeFrame1()
      Private BarCodeFrame2 As Frame = CreateBarCodeFrame2()
      Private CodeFrame As Frame = CreateCodeFrame()

      Public Sub New(barCodeFont As Font, codeFont As Font)
        Me.BarCodeFont = barCodeFont
        Me.CodeFont = codeFont
      End Sub

      Public Function Create(code As String) As Panel
        Dim panel As Panel = CreatePanel()
        panel.Controls.Add(CreateBarCodeLabel2(code))
        panel.Controls.Add(CreateBarCodeLabel1(code))
        panel.Controls.Add(CreateCodeLabel(code))
        Return panel
      End Function

      Private Structure Frame
        Dim X As Integer
        Dim Y As Integer
        Dim Width As Integer
        Dim Height As Integer
      End Structure

      Private Function CreatePanel() As Panel
        Dim panel As New Panel
        panel.Top = PanelFrame.Y
        panel.Left = PanelFrame.X
        panel.Width = PanelFrame.Width
        panel.Height = PanelFrame.Height
        Return panel
      End Function

      Private Function CreateBarCodeLabel1(code As String) As Label
        If BarCodeFont.Name = "NW-7" Then
          Return CreateLabel(code, BarCodeFont, BarCodeFrame1)
        Else
          Throw New Exception("フォントが不正な値です。")
        End If
      End Function

      Private Function CreateBarCodeLabel2(code As String) As Label
        If BarCodeFont.Name = "NW-7" Then
          Return CreateLabel(code, BarCodeFont, BarCodeFrame2)
        Else
          Throw New Exception("フォントが不正な値です。")
        End If
      End Function

      Private Function CreateCodeLabel(code As String) As Label
        Return CreateLabel(code, CodeFont, CodeFrame)
      End Function

      Private Function CreateLabel(code As String, font As Font, frame As Frame) As Label
        Dim label As New Label()

        label.Font = font
        label.Top = frame.Y
        Dim size As SizeF = label.CreateGraphics.MeasureString(code, font, PanelFrame.Width + 10)
        label.Left = (PanelFrame.Width - size.Width) / 2
        label.Width = frame.Width
        label.Height = frame.Height
        label.Text = code
        'label.FlatStyle = FlatStyle.Standard
        'label.BackColor = Color.Transparent

        If size.Width > label.Width Then
          label.ForeColor = Color.Red
        End If

        Return label
      End Function

      Private Function CreatePanelFrame() As Frame
        'Return CreateFrame(4, 4, 177, 75)
        Return CreateFrame(4, 4, 448, 157)
      End Function

      Private Function CreateBarCodeFrame1() As Frame
        'Return CreateFrame(0, 12, 170, 19)
        Return CreateFrame(0, 22, 400, 48)
      End Function

      Private Function CreateBarCodeFrame2() As Frame
        'Return CreateFrame(0, 27, 170, 19)
        Return CreateFrame(0, 54, 400, 48)
      End Function

      Private Function CreateCodeFrame() As Frame
        'Return CreateFrame(0, 46, 170, 19)
        Return CreateFrame(0, 96, 400, 40)
      End Function

      Private Function CreateFrame(x As Integer, y As Integer, w As Integer, h As Integer) As Frame
        Dim f As Frame
        With f
          .X = x
          .Y = y
          .Width = w
          .Height = h
        End With
        Return f
      End Function

    End Class

  End Namespace

End Namespace
