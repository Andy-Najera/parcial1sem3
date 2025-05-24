using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Presentation;
using A = DocumentFormat.OpenXml.Drawing;
using P = DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml;
using System.IO;

namespace proyecto1
{
    public partial class Form1 : Form
    {
        private const string GroqEndpoint = "https://api.groq.com/openai/v1/chat/completions";
        private const string GroqApiKey = "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"; 
        private const string Model = "llama3-70b-8192";

        public Form1()
        {
            InitializeComponent();
        }

        private async void buttonConsultar_Click(object sender, EventArgs e)
        {
            string consulta = textBoxConsultaIA.Text.Trim();

            if (string.IsNullOrEmpty(consulta))
            {
                MessageBox.Show("Por favor, ingresa tu consulta.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Fuerza la respuesta en español
            string consultaForzada = consulta + "\nResponde siempre en español.";

            textBoxResultadoAI.Text = "Cargando...";

            try
            {
                string respuesta = await ObtenerRespuestaGroq(consultaForzada);
                textBoxResultadoAI.Text = respuesta;

                // Guardar automáticamente en la base de datos
                GuardarEnBaseDeDatos(consulta, respuesta);
                GenerarArchivos(consulta, respuesta);
            }
            catch (Exception ex)
            {
                textBoxResultadoAI.Text = $"Error: {ex.Message}";
            }
        }

        private async Task<string> ObtenerRespuestaGroq(string prompt)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GroqApiKey);

                var requestData = new
                {
                    model = Model,
                    messages = new[]
                    {
                        new { role = "user", content = prompt }
                    }
                };

                string json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                int maxRetries = 3;
                int delay = 2000; // milisegundos

                for (int i = 0; i < maxRetries; i++)
                {
                    var response = await client.PostAsync(GroqEndpoint, content);

                    if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                    {
                        await Task.Delay(delay);
                        delay *= 2; // Espera exponencial
                        continue;
                    }

                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic resultado = JsonConvert.DeserializeObject(responseBody);
                    return resultado.choices[0].message.content.ToString().Trim();
                }

                throw new Exception("Se superó el límite de reintentos por demasiadas solicitudes (429).");
            }
        }

        private void GuardarEnBaseDeDatos(string consulta, string respuesta)
        {
            string connectionString = "Server=LAPTOP-0I8HCQGL\\SQLEXPRESS;Database=proyecto1;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "INSERT INTO ConsultasOpenAI (Fecha, Consulta, Respuesta) VALUES (@Fecha, @Consulta, @Respuesta)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Fecha", DateTime.Now);
                        command.Parameters.AddWithValue("@Consulta", consulta);
                        command.Parameters.AddWithValue("@Respuesta", respuesta);

                        command.ExecuteNonQuery();

                        MessageBox.Show("Datos guardados con éxito en la base de datos.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar en la base de datos: {ex.Message}", "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void GenerarArchivos(string consulta, string respuesta)
        {
            // Cambia esta ruta a la que prefieras
            string carpeta = @"C:\Users\andru\OneDrive\Desktop\PROYECTO1\Powerpoint-word"; 
                                                                                         
                                                                                         // string carpeta = Path.Combine("D:\\", "Backups", "ConsultasIA"); // Para otra unidad

            if (!Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);

            string nombreBase = CrearNombreValido(consulta);
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string sufijo = "";
            int contador = 1;

            string rutaWord, rutaPptx;

            // Buscar un nombre que no exista
            do
            {
                string nombreArchivo = $"{nombreBase}_{timestamp}{sufijo}";
                rutaWord = Path.Combine(carpeta, nombreArchivo + ".docx");
                rutaPptx = Path.Combine(carpeta, nombreArchivo + ".pptx");

                sufijo = $"_{contador}";
                contador++;
            }
            while (File.Exists(rutaWord) || File.Exists(rutaPptx));

            CrearDocumentoWordOpenXml(rutaWord, consulta, respuesta);
            CrearPresentacionPowerPointOpenXml(rutaPptx, consulta, respuesta);

        }

        private void CrearDocumentoWordOpenXml(string ruta, string consulta, string respuesta)
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(ruta, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = new Body();

                // Agrega la consulta con formato de título
                body.Append(
                    new Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text("Consulta:")))
                    {
                        ParagraphProperties = new ParagraphProperties(new Bold())
                    },
                    new Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text(consulta))),

                    // Agrega la respuesta con formato de título
                    new Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text("Respuesta:")))
                    {
                        ParagraphProperties = new ParagraphProperties(new Bold())
                    },
                    new Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text(respuesta)))
                );

                mainPart.Document.Append(body);
                mainPart.Document.Save();
            }
        }

        private void CrearPresentacionPowerPointOpenXml(string ruta, string consulta, string respuesta)
        {
            using (PresentationDocument presentationDocument = PresentationDocument.Create(ruta, PresentationDocumentType.Presentation))
            {
                PresentationPart presentationPart = presentationDocument.AddPresentationPart();
                presentationPart.Presentation = new P.Presentation();

                SlidePart slidePart1 = AddSlide(presentationPart, "Consulta", consulta);
                SlidePart slidePart2 = AddSlide(presentationPart, "Respuesta", respuesta);

                presentationPart.Presentation.SlideIdList = new SlideIdList(
                    new SlideId() { Id = 256U, RelationshipId = presentationPart.GetIdOfPart(slidePart1) },
                    new SlideId() { Id = 257U, RelationshipId = presentationPart.GetIdOfPart(slidePart2) }
                );

                presentationPart.Presentation.Save();
            }
        }
        private SlidePart AddSlide(PresentationPart presentationPart, string titulo, string contenido)
        {
            SlidePart slidePart = presentationPart.AddNewPart<SlidePart>();
            slidePart.Slide = new P.Slide(
                new P.CommonSlideData(
                    new P.ShapeTree(
                        new P.NonVisualGroupShapeProperties(
                            new P.NonVisualDrawingProperties() { Id = 1, Name = "Title" },
                            new P.NonVisualGroupShapeDrawingProperties(),
                            new ApplicationNonVisualDrawingProperties()
                        ),
                        new P.GroupShapeProperties(new A.TransformGroup()),
                        CreateTextShape(2, titulo, 0, 0, 7200000, 1000000),
                        CreateTextShape(3, contenido, 0, 1000000, 7200000, 4000000)
                    )
                )
            );
            return slidePart;
        }
        private P.Shape CreateTextShape(uint id, string text, int x, int y, int cx, int cy)
        {
            return new P.Shape(
                new P.NonVisualShapeProperties(
                    new P.NonVisualDrawingProperties() { Id = id, Name = "TextBox " + id },
                    new P.NonVisualShapeDrawingProperties(new A.ShapeLocks() { NoGrouping = true }),
                    new ApplicationNonVisualDrawingProperties()
                ),
                new P.ShapeProperties(new A.Transform2D(
                    new A.Offset() { X = x, Y = y },
                    new A.Extents() { Cx = cx, Cy = cy })),
                new P.TextBody(
                    new A.BodyProperties(),
                    new A.ListStyle(),
                    new A.Paragraph(new A.Run(new A.Text(text)))
                )
            );
        }

        private string CrearNombreValido(string texto)
        {
            // Reemplazar caracteres no válidos para nombres de archivo
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                texto = texto.Replace(c, '_');
            }
            return texto;
        }



    }
}