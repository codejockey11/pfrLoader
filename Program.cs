using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace pfrLoader
{
    class Program
    {
        static Char[] recordType_001_04 = new Char[04];
        static Char[] origin_005_05 = new Char[05];
        static Char[] destination_10_05 = new Char[05];
        static Char[] type_15_03 = new Char[03];

        static Char[] wpIdent_23_48 = new Char[48];
        static Char[] wpType_71_07 = new Char[07];

        static Int32 c = 1;

        static String dp = "";
        static String star = "";

        static List<String> wp = new List<String>();

        static StreamWriter ofilePFR = new StreamWriter("pfrRoutes.txt");

        static void Main(String[] args)
        {
            String userprofileFolder = Environment.GetEnvironmentVariable("USERPROFILE");
            String[] fileEntries = Directory.GetFiles(userprofileFolder + "\\Downloads\\", "28DaySubscription*.zip");

            ZipArchive archive = ZipFile.OpenRead(fileEntries[0]);
            ZipArchiveEntry entry = archive.GetEntry("PFR.txt");
            entry.ExtractToFile("PFR.txt", true);

            StreamReader file = new StreamReader("PFR.txt");

            String rec = file.ReadLine();

            while (!file.EndOfStream)
            {
                ProcessRecord(rec);
                rec = file.ReadLine();
            }

            ProcessRecord(rec);

            ofilePFR.Write(dp);
            ofilePFR.Write("~");

            if (wp.Count > 0)
            {
                for (Int32 x = 0; x < (wp.Count - 1); x++)
                {
                    ofilePFR.Write(wp[x]);
                    ofilePFR.Write(" ");
                }
                ofilePFR.Write(wp[wp.Count - 1]);
            }
            ofilePFR.Write("~");

            ofilePFR.Write(star);
            ofilePFR.Write(ofilePFR.NewLine);

            file.Close();

            ofilePFR.Close();
        }

        static void ProcessRecord(String record)
        {
            recordType_001_04 = record.ToCharArray(0, 4);

            String rt = new String(recordType_001_04);

            Int32 r = String.Compare(rt, "PFR1");
            if (r == 0)
            {
                if (c == 1)
                {
                    origin_005_05 = record.ToCharArray(04, 05);
                    String s = new String(origin_005_05).Trim();
                    ofilePFR.Write(s);
                    ofilePFR.Write("~");

                    destination_10_05 = record.ToCharArray(09, 05);
                    s = new String(destination_10_05).Trim();
                    ofilePFR.Write(s);
                    ofilePFR.Write("~");

                    type_15_03 = record.ToCharArray(14, 03);
                    s = new String(type_15_03).Trim();
                    ofilePFR.Write(s);
                    ofilePFR.Write("~");

                    c = 2;
                }
                else
                {
                    ofilePFR.Write(dp);
                    ofilePFR.Write("~");

                    if (wp.Count > 0)
                    {
                        for (Int32 x = 0; x < (wp.Count - 1); x++)
                        {
                            ofilePFR.Write(wp[x]);
                            ofilePFR.Write(" ");
                        }
                        ofilePFR.Write(wp[wp.Count - 1]);
                    }
                    ofilePFR.Write("~");

                    ofilePFR.Write(star);
                    ofilePFR.Write(ofilePFR.NewLine);

                    wp.Clear();

                    dp = "";
                    star = "";

                    origin_005_05 = record.ToCharArray(04, 05);
                    String s = new String(origin_005_05).Trim();
                    ofilePFR.Write(s);
                    ofilePFR.Write("~");

                    destination_10_05 = record.ToCharArray(09, 05);
                    s = new String(destination_10_05).Trim();
                    ofilePFR.Write(s);
                    ofilePFR.Write("~");

                    type_15_03 = record.ToCharArray(14, 03);
                    s = new String(type_15_03).Trim();
                    ofilePFR.Write(s);
                    ofilePFR.Write("~");
                }
            }

            r = String.Compare(rt, "PFR2");
            if (r == 0)
            {
                wpIdent_23_48 = record.ToCharArray(22, 48);
                wpType_71_07 = record.ToCharArray(70, 07);
                String s = new String(wpType_71_07).Trim();

                switch (s)
                {
                    case "DP":
                        {
                            dp = new String(wpIdent_23_48).Trim().Replace(" (RNAV)", "");
                            break;
                        }

                    case "STAR":
                        {
                            star = new String(wpIdent_23_48).Trim().Replace(" (RNAV)", "");
                            break;
                        }

                    case "AIRWAY":
                    case "FIX":
                    case "NAVAID":
                        {
                            wp.Add(new String(wpIdent_23_48).Trim());
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }
            }

        }
    }

}
