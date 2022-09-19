using NetBarcode;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
// using Color = System.Drawing.Color;
// using Font = System.Drawing.Font;
// using Graphics = System.Drawing.Graphics;
using UnityEngine;
using System.IO;
using System;
using Barcode = NetBarcode.Barcode;

public class Printer
{
    // string path = null;
    // string gameTime;
    // string gameDate;
    // string serviceOperatorRate;
    // string totalSelectedSpots;
    // string poolPrize;
    // string totalAmount;
    // string gId;
    // string barcodeNo;
    // string serviseCharge;
    // string drawTime;
    // string gameName;
    // int height = 600;
    // int tbets;
    // Dictionary<string, BetInfo> bets;


    // public Printer(string gameTime, string gameDate,
    //     string totalSelectedSpots,
    //     string drawTime,
    //     string barcodeNo, Dictionary<string, BetInfo> bets, float poolPrize, float serviseCharge, GameIds gameIds, int tbets)
    // {
    //     this.gameTime = gameTime;
    //     this.gameDate = gameDate;
    //     this.totalSelectedSpots = bets.Count.ToString();
    //     this.totalAmount = bets.Values.Sum(x => x.amount).ToString();
    //     this.barcodeNo = barcodeNo;
    //     this.poolPrize = poolPrize.ToString();
    //     this.bets = bets;
    //     this.tbets = tbets;
    //     this.serviseCharge = serviseCharge.ToString();
    //     this.drawTime = drawTime;
    //     path = Application.dataPath + "/Ticket.pdf";

    //     switch (gameIds)
    //     {
    //         case GameIds.cricket:
    //             gameName = "Rajwin-9";
    //             break;
    //         case GameIds.football:
    //             gameName = "RajwinRoyal-9";
    //             break;
    //         case GameIds.hockey:
    //             gameName = "RR-9";
    //             break;
    //         default:
    //             break;
    //     }

    // }

    // public void Print()
    // {

    //     int lines = bets.Count;
    //     height = ((int)(lines / 2)) * 20 + 400;
    //     var doc = new PrintDocument();
    //     doc.DefaultPageSettings.PaperSize = new PaperSize("Custom", 200, height);
    //     doc.DefaultPageSettings.Landscape = false;
    //     string directory = Application.dataPath;
    //     doc.DefaultPageSettings.Margins = new Margins(60, 40, 20, 20);
    //     //doc.PrinterSettings.PrinterName = "Microsoft Print to PDF";

    //     string file = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds.ToString();
    //         //doc.DefaultPageSettings.PrinterSettings.PrintFileName
    //         //    = Path.Combine(directory, file + ".pdf");
    //         doc.PrintPage += new PrintPageEventHandler(ProvideContent);
    //         doc.Print();

    // }


    // void ProvideContent(object sender, PrintPageEventArgs e)
    // {
    //     Graphics graphics = e.Graphics;
    //     Font font = new Font("Trebuchet MS", 10);

    //     float fontHeight = font.GetHeight();
    //     System.Drawing.Image image;
    //     int startX = 0;
    //     int startY = 0;
    //     int Offset = 20;
    //     string fontName = "Trebuchet MS";


    //     graphics.DrawString($"           {gameName}\nGame Acknowledgement", new Font("Impact", 14),
    //                         new SolidBrush(Color.Black), startX + Offset / 4, startY + Offset);
    //     Offset = Offset + 35;

    //     graphics.DrawString(" \n ", new Font(fontName, 10),
    //                       new SolidBrush(Color.Black), startX + Offset / 4, startY + Offset);
    //     Offset = Offset + 25;

    //     string data = $"Date: {gameDate}";
    //     graphics.DrawString(data,
    //                 new Font(fontName, 10),
    //                 new SolidBrush(Color.Black), startX, startY + Offset);
    //     Offset = Offset + 20;


    //     string time = $"Time: {gameTime}";
    //     graphics.DrawString(time,
    //                 new Font(fontName, 10),
    //                 new SolidBrush(Color.Black), startX, startY + Offset);



    //     Offset = Offset + 20;
    //     string dt = $"Contest Time: {drawTime}";
    //     graphics.DrawString(dt,
    //                 new Font(fontName, 10),
    //                 new SolidBrush(Color.Black), startX, startY + Offset);

    //     Offset = Offset + 20;

    //     string poolP = $"Pool Prize: {poolPrize}";
    //     graphics.DrawString(poolP,
    //                 new Font(fontName, 10),
    //                 new SolidBrush(Color.Black), startX, startY + Offset);

    //     Offset = Offset + 20;


    //     string ServiseCharge = $"Service Charge : {serviseCharge}";
    //     graphics.DrawString(ServiseCharge,
    //                 new Font(fontName, 10),
    //                 new SolidBrush(Color.Black), startX, startY + Offset);


    //     string underLine = "-------------------------------";

    //     Offset = Offset + 15;
    //     graphics.DrawString(underLine, new Font("Tahoma", 14, System.Drawing.FontStyle.Regular),
    //                 new SolidBrush(Color.Black), startX, startY + Offset);

    //     Offset = Offset + 25;
    //     int startingPoint = -5;
    //     int fountsize = 7;
    //     string Heading = " Num QTY   P     Num QTY   P    Num QTY   P";
    //     graphics.DrawString(Heading, new Font("Consolas ", fountsize),
    //                     new SolidBrush(Color.Black), startingPoint, startY + Offset);
    //     Offset = Offset + 2;

    //     string totalbets = string.Empty;
    //     int i = 0;
    //     int index = 0;
    //     foreach (var item in bets)
    //     {
    //         string qty = string.Empty;//amount
    //         switch (item.Value.qty.ToString().ToCharArray().Length)
    //         {
    //             case 1: qty = item.Value.qty.ToString() + "  "; break;
    //             case 2: qty = item.Value.qty.ToString() + " "; break;
    //             case 3: qty = item.Value.qty.ToString() + ""; break;

    //         }
    //         totalbets += string.Concat($" {item.Key}: {qty} {item.Value.singleTicketPrize}", " ");
    //         index++;
    //         if (index == bets.Count)
    //         {
    //             Offset = Offset + 20;
    //             graphics.DrawString(totalbets, new Font("Consolas", fountsize),
    //                 new SolidBrush(Color.Black), startingPoint, startY + Offset);
    //             break;
    //         }
    //         if (index % 3 != 0)
    //         {
    //             continue;
    //         };
    //         Offset = Offset + 20;
    //         graphics.DrawString(totalbets, new Font("Consolas", fountsize),
    //                      new SolidBrush(Color.Black), startingPoint, startY + Offset);

    //         totalbets = string.Empty;

    //         i++;


    //     }

    //     Offset = Offset + 10;
    //     underLine = "-------------------------------";
    //     graphics.DrawString(underLine, new Font(fontName, 14),
    //                 new SolidBrush(Color.Black), startX, startY + Offset);
    //     Offset = Offset + 20;
    //     string spots = $"Total Selected Spots : {totalSelectedSpots}";
    //     graphics.DrawString(spots, new Font("Impact", 10),
    //               new SolidBrush(Color.Black), startX, startY + Offset);

    //     Offset = Offset + 20;
    //     string amount = $"Total Amount ₹ {totalAmount}";
    //     graphics.DrawString(amount, new Font("Impact", 10),
    //               new SolidBrush(Color.Black), startX, startY + Offset);

    //     Offset = Offset + 30;
    //     var barcode = new Barcode(barcodeNo);
    //     graphics.DrawImage(barcode.GetImage(), startX + 25, startY + Offset, 100, 25);

    //     Offset = Offset + 30;

    //     string serialNo = barcodeNo;
    //     graphics.DrawString(serialNo, new Font(fontName, 10),
    //                new SolidBrush(Color.Black), startX + 25, startY + Offset);
    // }
    // void SaveImage(Graphics graphics)
    // {
    //     int x = (int)graphics.DpiX;
    //     int y = (int)graphics.DpiX;
    //     Bitmap bmpPicture = new Bitmap(x, y);

    //     graphics.DrawImage(bmpPicture, x, y);
    //     Bitmap bitmap = new Bitmap((int)graphics.DpiX, (int)graphics.DpiY, graphics);
    //     Graphics g = Graphics.FromImage(bitmap);
    //     g.DrawImage(bitmap, 0, 0);

    //     g.Clear(Color.Green);

    // }

}
// public class PrintText
// {
//     public PrintText(string text, Font font) : this(text, font, new StringFormat()) { }

//     public PrintText(string text, Font font, StringFormat stringFormat)
//     {
//         Text = text;
//         Font = font;
//         StringFormat = stringFormat;
//     }

//     public string Text { get; set; }

//     public Font Font { get; set; }

//     /// <summary> Default is horizontal string formatting </summary>
//     public StringFormat StringFormat { get; set; }
// }