using System;
using System.Drawing;
using System.Dynamic;
using System.IO;
using GP.DAL.Data.Models;
using System.Net.Http.Headers;
using GP_BLL.Interfaces;
using Python.Runtime;
using Newtonsoft.Json;
using SkiaSharp;

namespace GP_BLL.Services
{
    public class SpineProcessingService : ISpineProcessingService
    {


        public void ProcessSpine(string glbPath, string stlPath, string outputDirectory)
        {
            ConfigurePythonEnvironment();

            if (!File.Exists(glbPath))
            {
                throw new FileNotFoundException($"GLB file not found at: {glbPath}");
            }

            using (Py.GIL())
            {
                try
                {
                    dynamic sys = Py.Import("sys");
                    string moduleDir = @"E:\Year 4\sem2\gp\final";
                    sys.path.append(moduleDir);

                    dynamic spineProcessing = Py.Import("Script");

                    spineProcessing.process_spine_image(glbPath, stlPath, outputDirectory);
                }
                catch (PythonException ex)
                {
                    throw new Exception($"Python error: {ex.Message}\nStack trace: {ex.StackTrace}");
                }
            }
        }

        private void ConfigurePythonEnvironment()
        {
            try
            {
                string pythonHome = @"C:\Users\abdod\AppData\Local\Programs\Python\Python37";
                string pythonPath = $"{pythonHome};{Path.Combine(pythonHome, "Lib")};{Path.Combine(pythonHome, "DLLs")};";

                Environment.SetEnvironmentVariable("PYTHONHOME", pythonHome);
                Environment.SetEnvironmentVariable("PATH", $"{pythonPath};{Environment.GetEnvironmentVariable("PATH")}");
                Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", Path.Combine(pythonHome, "python37.dll"));

                if (!PythonEngine.IsInitialized)
                {
                    PythonEngine.Initialize();
                    PythonEngine.BeginAllowThreads();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Python configuration failed: {ex.Message}");
            }
        }
        private const string API_URL = "https://88e5-156-207-207-114.ngrok-free.app/daowd_3m_a3mam_alshr8yh";

        public async Task<MedicalReport> ProcessBackImageAsync(string imagePath, string outputFolder)
        {
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException($"Image file not found at: {imagePath}");
            }

            dynamic apiResponse = await GetPredictionAsync(imagePath, Path.Combine(outputFolder, "response.json"));
            var report = ProcessApiResponse(apiResponse);
            return report;
        }

        public void AnnotateAndSaveImage(MedicalReport report, string inputImagePath, string outputImagePath)
        {
            if (report?.MidHip == null)
            {
                throw new ArgumentException("Cannot annotate image due to missing MidHip keypoint.");
            }

            using var originalImage = SKBitmap.Decode(inputImagePath);
            if (originalImage == null)
            {
                throw new InvalidOperationException($"Failed to load image from {inputImagePath}.");
            }

            using var surface = SKSurface.Create(new SKImageInfo(originalImage.Width, originalImage.Height));
            var canvas = surface.Canvas;
            canvas.DrawBitmap(originalImage, 0, 0);

            var pointPaint = new SKPaint { Color = SKColors.Red, Style = SKPaintStyle.Fill };
            var linePaint = new SKPaint { Color = SKColors.Blue, StrokeWidth = 2 };
            var textPaint = new SKPaint { Color = SKColors.White, TextSize = 14 };
            var midlinePaint = new SKPaint { Color = SKColors.Yellow, StrokeWidth = 2 };

            float midlineX = report.MidHip.X;
            canvas.DrawLine(midlineX, 0, midlineX, originalImage.Height, midlinePaint);

            DrawKeypoint(canvas, report.C7, midlineX, pointPaint, linePaint, textPaint);
            DrawKeypoint(canvas, report.T7, midlineX, pointPaint, linePaint, textPaint);
            DrawKeypoint(canvas, report.LeftHip, midlineX, pointPaint, linePaint, textPaint);
            DrawKeypoint(canvas, report.RightHip, midlineX, pointPaint, linePaint, textPaint);
            DrawKeypoint(canvas, report.MidHip, midlineX, pointPaint, linePaint, textPaint);
            DrawKeypoint(canvas, report.LeftScapula, midlineX, pointPaint, linePaint, textPaint);
            DrawKeypoint(canvas, report.RightScapula, midlineX, pointPaint, linePaint, textPaint);
            DrawKeypoint(canvas, report.LeftShoulder, midlineX, pointPaint, linePaint, textPaint);
            DrawKeypoint(canvas, report.RightShoulder, midlineX, pointPaint, linePaint, textPaint);
            DrawKeypoint(canvas, report.LeftSide, midlineX, pointPaint, linePaint, textPaint);
            DrawKeypoint(canvas, report.RightSide, midlineX, pointPaint, linePaint, textPaint);
            DrawKeypoint(canvas, report.LeftUnderArm, midlineX, pointPaint, linePaint, textPaint);
            DrawKeypoint(canvas, report.RightUnderArm, midlineX, pointPaint, linePaint, textPaint);

            Directory.CreateDirectory(Path.GetDirectoryName(outputImagePath));
            using var image = surface.Snapshot();
            using var data = image.Encode(SKEncodedImageFormat.Jpeg, 90);
            using var stream = File.OpenWrite(outputImagePath);
            data.SaveTo(stream);
        }

        private async Task<dynamic> GetPredictionAsync(string imagePath, string savePath)
        {
            using var client = new HttpClient();
            using var content = new MultipartFormDataContent();
            var imageContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
            imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
            content.Add(imageContent, "image", Path.GetFileName(imagePath));

            var response = await client.PostAsync(API_URL, content);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            File.WriteAllText(savePath, jsonResponse);

            return JsonConvert.DeserializeObject<ExpandoObject>(jsonResponse);
        }

        private MedicalReport ProcessApiResponse(dynamic response)
        {
            var report = new MedicalReport();

            if (response?.models == null)
            {
                throw new InvalidOperationException("Invalid API response structure.");
            }

            foreach (var model in response.models)
            {
                foreach (var prediction in model.predictions)
                {
                    if (prediction.@class == "Back Keypoints" && prediction.keypoints?.Count >= 7)
                    {
                        report.C7 = CreateKeypoint("C7", prediction.keypoints[0]);
                        report.T7 = CreateKeypoint("T7", prediction.keypoints[1]);
                        report.LeftHip = CreateKeypoint("LeftHip", prediction.keypoints[2]);
                        report.RightHip = CreateKeypoint("RightHip", prediction.keypoints[3]);
                        report.MidHip = CalculateMidHip(report.LeftHip, report.RightHip);
                        report.LeftScapula = CreateKeypoint("LeftScapula", prediction.keypoints[5]);
                        report.RightScapula = CreateKeypoint("RightScapula", prediction.keypoints[6]);
                    }
                    else if (prediction.@class == "pointofside" && prediction.keypoints?.Count >= 6)
                    {
                        report.LeftShoulder = CreateKeypoint("LeftShoulder", prediction.keypoints[0]);
                        report.RightShoulder = CreateKeypoint("RightShoulder", prediction.keypoints[1]);
                        report.LeftSide = CreateKeypoint("LeftSide", prediction.keypoints[2]);
                        report.RightSide = CreateKeypoint("RightSide", prediction.keypoints[3]);
                        report.LeftUnderArm = CreateKeypoint("LeftUnderArm", prediction.keypoints[4]);
                        report.RightUnderArm = CreateKeypoint("RightUnderArm", prediction.keypoints[5]);
                    }
                }
            }

            if (report.C7 == null || report.MidHip == null || report.LeftHip == null || report.RightHip == null ||
                report.LeftShoulder == null || report.RightShoulder == null || report.LeftSide == null ||
                report.RightSide == null || report.LeftUnderArm == null || report.RightUnderArm == null)
            {
                throw new InvalidOperationException("Missing required keypoints in API response.");
            }

            // Calculate FAI metrics
            float a = EuclideanDistance(report.LeftUnderArm, report.MidHip);
            float b = EuclideanDistance(report.RightUnderArm, report.MidHip);
            float C7_Mid = Math.Abs(report.C7.X - report.MidHip.X);
            report.FAI_C7 = (a + b != 0) ? (C7_Mid / (a + b)) * 100 : 0;
            report.FAI_A = (a + b != 0) ? (Math.Abs(a - b) / (a + b)) * 100 : 0;
            float c = EuclideanDistance(report.LeftSide, report.MidHip);
            float d = EuclideanDistance(report.RightSide, report.MidHip);
            report.FAI_T = (c + d != 0) ? (Math.Abs(c - d) / (c + d)) * 100 : 0;

            // Calculate HDI metrics
            float trunkHeight = VerticalDisplacement(report.C7, report.MidHip);
            float e = VerticalDisplacement(report.LeftShoulder, report.RightShoulder);
            report.HDI_S = (trunkHeight != 0) ? (e / trunkHeight) * 100 : 0;
            float f = VerticalDisplacement(report.LeftUnderArm, report.RightUnderArm);
            report.HDI_A = (trunkHeight != 0) ? (f / trunkHeight) * 100 : 0;
            float g = VerticalDisplacement(report.LeftSide, report.RightSide);
            report.HDI_T = (trunkHeight != 0) ? (g / trunkHeight) * 100 : 0;

            return report;
        }

        private Keypoint CreateKeypoint(string name, dynamic keypoint)
        {
            if (keypoint?.x == null || keypoint?.y == null)
            {
                throw new InvalidOperationException($"Invalid keypoint data for {name}.");
            }

            return new Keypoint
            {
                Name = name,
                X = (float)keypoint.x,
                Y = (float)keypoint.y
            };
        }

        private Keypoint CalculateMidHip(Keypoint leftHip, Keypoint rightHip)
        {
            if (leftHip == null || rightHip == null)
            {
                throw new InvalidOperationException("Cannot calculate MidHip due to missing hip keypoints.");
            }

            return new Keypoint
            {
                Name = "MidHip",
                X = (leftHip.X + rightHip.X) / 2,
                Y = (leftHip.Y + rightHip.Y) / 2
            };
        }

        private void DrawKeypoint(SKCanvas canvas, Keypoint kp, float midlineX, SKPaint pointPaint, SKPaint linePaint, SKPaint textPaint)
        {
            if (kp == null)
                return;

            if (kp.Name.Contains("Side") || kp.Name.Contains("UnderArm"))
            {
                canvas.DrawLine(kp.X, kp.Y, midlineX, kp.Y, linePaint);
            }
            canvas.DrawCircle(kp.X, kp.Y, 5, pointPaint);
            canvas.DrawText($"{kp.Name} ({(int)kp.X},{(int)kp.Y})", kp.X + 10, kp.Y - 5, textPaint);
        }

        private float EuclideanDistance(Keypoint p1, Keypoint p2)
        {
            return (float)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        private float VerticalDisplacement(Keypoint p1, Keypoint p2)
        {
            return Math.Abs(p1.Y - p2.Y);
        }

        private float HorizontalDisplacement(Keypoint p1, Keypoint p2)
        {
            return Math.Abs(p1.X - p2.X);
        }
    }
}
        public class MedicalReport
        {
            public Keypoint C7 { get; set; }
            public Keypoint T7 { get; set; }
            public Keypoint LeftHip { get; set; }
            public Keypoint RightHip { get; set; }
            public Keypoint MidHip { get; set; }
            public Keypoint LeftScapula { get; set; }
            public Keypoint RightScapula { get; set; }
            public Keypoint LeftShoulder { get; set; }
            public Keypoint RightShoulder { get; set; }
            public Keypoint LeftSide { get; set; }
            public Keypoint RightSide { get; set; }
            public Keypoint LeftUnderArm { get; set; }
            public Keypoint RightUnderArm { get; set; }
            public float HDI_S { get; set; }
            public float HDI_A { get; set; }
            public float HDI_T { get; set; }
            public float FAI_C7 { get; set; }
            public float FAI_A { get; set; }
            public float FAI_T { get; set; }

        }
    


