using EMS.API.Image.Models;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace EMS.API.Image.Controllers
{
    public class ExcelController : Controller
    {
        public ActionResult ExportListCustomer(CustomerImportAddTempModel ListCustomer)
        {
            var FullPath = System.Web.HttpContext.Current.Server.MapPath("~/Assets/Excels/" + ListCustomer.FileName);
            var fileExt = Path.GetExtension(FullPath);
            HSSFWorkbook xls = new HSSFWorkbook();
            XSSFWorkbook xlsx = new XSSFWorkbook();
            ISheet sheet;
            IDataFormat dataFormatCustom;
            try
            {
                using (var fs = new FileStream(FullPath, FileMode.Open, FileAccess.ReadWrite))
                {
                    if (fileExt == ".xls")
                    {
                        xls = new HSSFWorkbook(fs);
                        sheet = xls.GetSheetAt(0);
                        dataFormatCustom = xls.CreateDataFormat();
                    }
                    else
                    {
                        xlsx = new XSSFWorkbook(fs);
                        sheet = xlsx.GetSheetAt(0);
                        dataFormatCustom = xlsx.CreateDataFormat();
                    }
                    sheet.GetRow(0).CreateCell(13).SetCellValue("Nguyên nhân lỗi");
                    var totalRow = sheet.LastRowNum;
                    var totalList = 0;
                    int rowIndex = 1;
                    int stt = 1;

                    if (ListCustomer.Type == 1)
                    {
                        //Chỉ lấy list lỗi
                        var ListFilter = ListCustomer.ListCustomerImportAddTempDetail.Where(m => m.IsSuccess == "N");
                        totalList = ListFilter.Count();
                        foreach (var item in ListFilter)
                        {
                            // tao row mới
                            var newRow = sheet.CreateRow(rowIndex);
                            // set giá trị
                            newRow.CreateCell(0).SetCellValue(stt++);
                            newRow.CreateCell(1).SetCellValue(item.FullName);
                            newRow.CreateCell(2).SetCellValue(item.Email);
                            newRow.CreateCell(3).SetCellValue(item.MobileNumber);
                            newRow.CreateCell(4).SetCellValue(item.ProvinceName);
                            newRow.CreateCell(5).SetCellValue(item.DistrictName);
                            newRow.CreateCell(6).SetCellValue(item.WardName);
                            newRow.CreateCell(7).SetCellValue(item.Address);
                            newRow.CreateCell(8).SetCellValue(item.BirthDay != null ? item.BirthDay.Split(' ')[0] : "");
                            newRow.CreateCell(9).SetCellValue(item.TaxCode);
                            newRow.CreateCell(10).SetCellValue(item.Channel);
                            newRow.CreateCell(11).SetCellValue(item.Note);
                            newRow.CreateCell(12).SetCellValue(item.Sticker);
                            newRow.CreateCell(13).SetCellValue(item.InformationError);

                            sheet.GetRow(rowIndex).GetCell(8).CellStyle.DataFormat = dataFormatCustom.GetFormat("dd/mm/yyyy");
                            // tăng index
                            rowIndex++;
                        }
                    }
                    else if (ListCustomer.Type == 2)
                    {
                        //Lấy tất cả
                        totalList = ListCustomer.ListCustomerImportAddTempDetail.Count();
                        foreach (var item in ListCustomer.ListCustomerImportAddTempDetail)
                        {
                            //HSSFRichTextString richString = new HSSFRichTextString(item.InformationError);

                            // tao row mới
                            var newRow = sheet.CreateRow(rowIndex);
                            // set giá trị
                            newRow.CreateCell(0).SetCellValue(stt++);
                            newRow.CreateCell(1).SetCellValue(item.FullName);
                            newRow.CreateCell(2).SetCellValue(item.Email);
                            newRow.CreateCell(3).SetCellValue(item.MobileNumber);
                            newRow.CreateCell(4).SetCellValue(item.ProvinceName);
                            newRow.CreateCell(5).SetCellValue(item.DistrictName);
                            newRow.CreateCell(6).SetCellValue(item.WardName);
                            newRow.CreateCell(7).SetCellValue(item.Address);
                            newRow.CreateCell(8).SetCellValue(item.BirthDay != null ? item.BirthDay.Split(' ')[0] : "");
                            newRow.CreateCell(9).SetCellValue(item.TaxCode);
                            newRow.CreateCell(10).SetCellValue(item.Channel);
                            newRow.CreateCell(11).SetCellValue(item.Note);
                            newRow.CreateCell(12).SetCellValue(item.Sticker);
                            newRow.CreateCell(13).SetCellValue(item.InformationError);

                            sheet.GetRow(rowIndex).GetCell(8).CellStyle.DataFormat = dataFormatCustom.GetFormat("dd/mm/yyyy");
                            // tăng index
                            rowIndex++;
                        }
                    }

                    if (totalRow > totalList)
                    {
                        for (var i = rowIndex; i <= totalRow; i++)
                        {
                            var newRow = sheet.CreateRow(i);
                            // set giá trị
                            newRow.CreateCell(0).SetCellValue("");
                            newRow.CreateCell(1).SetCellValue("");
                            newRow.CreateCell(2).SetCellValue("");
                            newRow.CreateCell(3).SetCellValue("");
                            newRow.CreateCell(4).SetCellValue("");
                            newRow.CreateCell(5).SetCellValue("");
                            newRow.CreateCell(6).SetCellValue("");
                            newRow.CreateCell(7).SetCellValue("");
                            newRow.CreateCell(8).SetCellValue("");
                            newRow.CreateCell(9).SetCellValue("");
                            newRow.CreateCell(10).SetCellValue("");
                            newRow.CreateCell(11).SetCellValue("");
                            newRow.CreateCell(12).SetCellValue("");
                            newRow.CreateCell(13).SetCellValue("");

                            sheet.GetRow(i).GetCell(8).CellStyle.DataFormat = dataFormatCustom.GetFormat("dd/mm/yyyy");
                        }
                    }
                }

                using (var fs = new FileStream(FullPath, FileMode.Open, FileAccess.ReadWrite))
                {
                    if (fileExt == ".xls")
                    {
                        xls.Write(fs);
                    }
                    else
                    {
                        xlsx.Write(fs);
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ExcelController.ExportExcelListCustomer: " + ex.Message);
            }

            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            if (fileExt == ".xls")
            {
                xls.Write(output);
            }
            else
            {
                xlsx.Write(output);
            }

            //Return the result to the end user
            return File(output.ToArray(),
             "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
             "DanhSachKhachHang");
        }
    }
}