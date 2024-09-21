using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Drawing;

namespace TDTU.API.Utilities;

public static class ExcelHelper
{
	public static string ExcelContentType
	{
		get
		{ return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; }
	}

	public static ExcelWorksheet GetStyle(ExcelWorksheet workSheet, int column)
	{
		try
		{
			workSheet.TabColor = System.Drawing.Color.Black;
			//border
			workSheet.Cells.Style.Border.Top.Style = ExcelBorderStyle.Thin;
			workSheet.Cells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
			workSheet.Cells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
			workSheet.Cells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
			//center vertical
			workSheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
			//freeze top row
			workSheet.View.FreezePanes(2, 1);

			//color columns
			for (int i = 1; i <= column; i++)
			{
				workSheet.Column(i).Style.Fill.PatternType = ExcelFillStyle.Solid;
				workSheet.Column(i).Style.Fill.BackgroundColor.SetColor(Color.FromArgb(237, 237, 237));
				workSheet.Column(i).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
			}

			// Setting the properties 
			// of the first row 
			workSheet.Row(1).Height = 30;
			workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
			workSheet.Row(1).Style.Font.Bold = true;
			workSheet.Row(1).Style.Fill.PatternType = ExcelFillStyle.Solid;
			workSheet.Row(1).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#305496"));
			workSheet.Row(1).Style.Font.Color.SetColor(ColorTranslator.FromHtml("#FFFFFF"));

			return workSheet;
		}
		catch (Exception ex)
		{
			return workSheet;
		}
	}
}
