using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using A = DocumentFormat.OpenXml.Drawing;
using P = DocumentFormat.OpenXml.Presentation;

namespace proyecto1
{
    public partial class Form1 : Form
    {

        private const string OpenAiEndpoint = "https://api.openai.com/v1/chat/completions";
        private const string OpenAiApiKey = "APIKEY";
        private const string Model = "gpt-4.1-mini";
        private const string OutputFolder = @"C:\Users\andru\OneDrive\Desktop\PROYECTO1\Powerpoint-word";
        private const string DbConnectionString = "Server=LAPTOP-0I8HCQGL\\SQLEXPRESS;Database=proyecto1;Integrated Security=True;";

        public Form1()
        {
            InitializeComponent();

            //this.buttonConsultar.Click += new EventHandler(this.buttonConsultar_Click);
            this.buttonWord.Click += new EventHandler(this.buttonWord_Click);
            this.buttonPp.Click += new EventHandler(this.buttonPp_Click);
            //this.textBoxConsultaIA.TextChanged += new EventHandler(this.textBoxConsultaIA_TextChanged);
        }


        private async void buttonConsultar_Click(object sender, EventArgs e)
        {
            string consulta = textBoxConsultaIA.Text.Trim();

            if (string.IsNullOrEmpty(consulta))
            {
                MessageBox.Show("Por favor, ingresa tu consulta.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            textBoxResultadoAI.Text = "Cargando...";

            try
            {
                string respuesta = await ObtenerRespuestaOpenAI(consulta);
                textBoxResultadoAI.Text = respuesta;

                await GuardarEnBaseDeDatos(consulta, respuesta);
            }
            catch (Exception ex)
            {
                textBoxResultadoAI.Text = $"Error: {ex.Message}";
            }
        }

        private void textBoxConsultaIA_TextChanged(object sender, EventArgs e)
        {

        }

        private async Task<string> ObtenerRespuestaOpenAI(string tema)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", OpenAiApiKey);

                var requestBody = new
                {
                    model = Model,
                    messages = new[]
                    {
                        new
                        {
                            role = "system",
                            content = "Eres un asistente académico que proporciona información precisa, bien estructurada y con fuentes confiables."
                        },
                        new
                        {
                            role = "user",
                            content = $@"Por favor, genera una respuesta detallada y académica en español sobre el tema: '{tema}'.
La respuesta debe estar dividida en las siguientes secciones:
Introducción:
Desarrollo:
Ejemplos:
Conclusiones:
Fuentes:
El formato debe ser profesional, con estructura clara y lenguaje técnico adecuado."
                        }
                    },
                    temperature = 0.7,
                    max_tokens = 1000,
                };

                var content = new StringContent(
                    JsonConvert.SerializeObject(requestBody),
                    Encoding.UTF8,
                    "application/json");

                int maxRetries = 3;
                int delayMs = 2000;

                for (int i = 0; i < maxRetries; i++)
                {
                    try
                    {
                        var response = await client.PostAsync(OpenAiEndpoint, content);

                        if (response.StatusCode == HttpStatusCode.TooManyRequests)
                        {
                            await Task.Delay(delayMs);
                            delayMs *= 2;
                            continue;
                        }

                        response.EnsureSuccessStatusCode();

                        var responseBody = await response.Content.ReadAsStringAsync();
                        dynamic result = JsonConvert.DeserializeObject(responseBody);
                        return result.choices[0].message.content.ToString().Trim();
                    }
                    catch (HttpRequestException) when (i < maxRetries - 1)
                    {
                        await Task.Delay(delayMs);
                        delayMs *= 2;
                    }
                }

                throw new Exception("Se superó el límite de reintentos por demasiadas solicitudes (429).");
            }
        }

        private async Task GuardarEnBaseDeDatos(string consulta, string respuesta)
        {
            try
            {
                using (var connection = new SqlConnection(DbConnectionString))
                using (var command = new SqlCommand(
                    "INSERT INTO ConsultasOpenAI (Fecha, Consulta, Respuesta) VALUES (@Fecha, @Consulta, @Respuesta)",
                    connection))
                {
                    await connection.OpenAsync();
                    command.Parameters.AddWithValue("@Fecha", DateTime.Now);
                    command.Parameters.AddWithValue("@Consulta", consulta);
                    command.Parameters.AddWithValue("@Respuesta", respuesta);
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar en la base de datos: {ex.Message}",
                    "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonWord_Click(object sender, EventArgs e)
        {
            string consulta = textBoxConsultaIA.Text.Trim();
            string respuesta = textBoxResultadoAI.Text.Trim();

            if (string.IsNullOrEmpty(consulta) || string.IsNullOrEmpty(respuesta))
            {
                MessageBox.Show("Primero realiza una consulta para generar el documento.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            GenerarDocumentoWord(consulta, respuesta);
        }

        private void buttonPp_Click(object sender, EventArgs e)
        {
            string consulta = textBoxConsultaIA.Text.Trim();
            string respuesta = textBoxResultadoAI.Text.Trim();

            if (string.IsNullOrEmpty(consulta) || string.IsNullOrEmpty(respuesta))
            {
                MessageBox.Show("Primero realiza una consulta para generar la presentación.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            GenerarPresentacionPowerPoint(consulta, respuesta);
        }

        private void GenerarDocumentoWord(string consulta, string respuesta)
        {
            string filePath = GenerarRutaArchivo(".docx");
            CrearDocumentoWord(filePath, consulta, respuesta);
            MessageBox.Show($"Documento Word generado en:\n{filePath}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void GenerarPresentacionPowerPoint(string consulta, string respuesta)
        {
            string filePath = GenerarRutaArchivo(".pptx");
            CrearPresentacionPowerPoint(filePath, consulta, respuesta);
            MessageBox.Show($"Presentación PowerPoint generada en:\n{filePath}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string GenerarRutaArchivo(string extension)
        {
            string nombreBase = CrearNombreValido(textBoxConsultaIA.Text);
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            Directory.CreateDirectory(OutputFolder);
            return Path.Combine(OutputFolder, $"{nombreBase}_{timestamp}{extension}");
        }

        private void CrearDocumentoWord(string ruta, string consulta, string respuesta)
        {
            using (var doc = WordprocessingDocument.Create(ruta, WordprocessingDocumentType.Document))
            {
                var mainPart = doc.AddMainDocumentPart();
                mainPart.Document = new Document();

                var body = new Body(
                    new Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text("")))
                    {
                        ParagraphProperties = new ParagraphProperties(new Bold())
                    },
                    new Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text(consulta))),
                    new Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text("")))
                    {
                        ParagraphProperties = new ParagraphProperties(new Bold())
                    },
                    new Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text(respuesta)))
                );

                mainPart.Document.Append(body);
            }
        }

        private void CrearPresentacionPowerPoint(string ruta, string consulta, string respuesta)
        {
            using (var presentation = PresentationDocument.Create(ruta, PresentationDocumentType.Presentation))
            {
                var presentationPart = presentation.AddPresentationPart();
                presentationPart.Presentation = new P.Presentation();

                var slidePart1 = AgregarDiapositiva(presentationPart, "", consulta);
                var slidePart2 = AgregarDiapositiva(presentationPart, "", respuesta);

                presentationPart.Presentation.SlideIdList = new SlideIdList(
                    new SlideId { Id = 256U, RelationshipId = presentationPart.GetIdOfPart(slidePart1) },
                    new SlideId { Id = 257U, RelationshipId = presentationPart.GetIdOfPart(slidePart2) }
                );
            }
        }

        private SlidePart AgregarDiapositiva(PresentationPart presentationPart, string titulo, string contenido)
        {
            var slidePart = presentationPart.AddNewPart<SlidePart>();
            slidePart.Slide = new P.Slide(
                new P.CommonSlideData(
                    new P.ShapeTree(
                        new P.NonVisualGroupShapeProperties(
                            new P.NonVisualDrawingProperties { Id = 1, Name = "Title" },
                            new P.NonVisualGroupShapeDrawingProperties(),
                            new ApplicationNonVisualDrawingProperties()
                        ),
                        new P.GroupShapeProperties(new A.TransformGroup()),
                        CrearCajaTexto(2, titulo, 0, 0, 7200000, 1000000),
                        CrearCajaTexto(3, contenido, 0, 1000000, 7200000, 4000000)
                    )
                )
            );
            return slidePart;
        }

        private P.Shape CrearCajaTexto(uint id, string text, int x, int y, int cx, int cy)
        {
            return new P.Shape(
                new P.NonVisualShapeProperties(
                    new P.NonVisualDrawingProperties { Id = id, Name = $"TextBox {id}" },
                    new P.NonVisualShapeDrawingProperties(new A.ShapeLocks { NoGrouping = true }),
                    new ApplicationNonVisualDrawingProperties()
                ),
                new P.ShapeProperties(new A.Transform2D(
                    new A.Offset { X = x, Y = y },
                    new A.Extents { Cx = cx, Cy = cy })),
                new P.TextBody(
                    new A.BodyProperties(),
                    new A.ListStyle(),
                    new A.Paragraph(new A.Run(new A.Text(text)))
                )
            );
        }

        private string CrearNombreValido(string texto)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                texto = texto.Replace(c, '_');
            }
            return texto;
        }

        private void buttonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}