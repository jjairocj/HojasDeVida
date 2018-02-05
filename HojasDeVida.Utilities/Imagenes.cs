using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Web;
using Abp.Extensions;

namespace HojasDeVida
{
    public enum ContenedoresImagenes
    {
        [Display(Name = "Imagen de perfil")]
        ImagenPerfil,
        [Display(Name = "Imagen convocatoria")]
        ImagenConvocatoria
    }

    public static class Imagenes
    {
        public static string CargarImagen(string usuarioId, HttpPostedFileBase archivo, ContenedoresImagenes contenedorImagen)
        {
            // Valida que el archivo sea una imagen

            if (!archivo.EsImagen())
            {
                return "El archivo que está intentando cargar no es una imagen válida.";
            }

            StorageCredentials storageCredentials = new StorageCredentials(ConfigurationManager.AppSettings["ImagesStorageAccountName"], ConfigurationManager.AppSettings["ImagesStorageAccountKey"]);
            CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, useHttps: true);

            try
            {
                //Imagenes de perfil: https://terecomiendoca.blob.core.windows.net/avatar/
                //Imagenes de convocatorias: https://terecomiendoca.blob.core.windows.net/logos/

                var nombreContenedor = contenedorImagen == ContenedoresImagenes.ImagenPerfil ? "avatar" : "logos";

                CloudBlobClient client = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference(nombreContenedor);

                container.CreateIfNotExists();

                container.SetPermissions(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });

                CloudBlockBlob blob = container.GetBlockBlobReference(usuarioId + "_original.jpg");
                blob.DeleteIfExists();
                blob.UploadFromStream(archivo.InputStream);

                Image image = Image.FromStream(archivo.InputStream);

                Image thumb = ResizeImage(image, new Size(240, 240));
                //Image thumb = image.GetThumbnailImage(240, 240, () => false, IntPtr.Zero);

                var stream = new MemoryStream();
                thumb.Save(stream, ImageFormat.Jpeg);
                stream.Position = 0;

                blob = container.GetBlockBlobReference(usuarioId + "_thumb.jpg");
                blob.DeleteIfExists();
                blob.UploadFromStream(stream);
            }
            catch (Exception)
            {
                //
            }

            return null;
        }

        public static void EliminarImagen(string usuarioId, ContenedoresImagenes contenedorImagen)
        {
            StorageCredentials storageCredentials = new StorageCredentials(ConfigurationManager.AppSettings["ImagesStorageAccountName"], ConfigurationManager.AppSettings["ImagesStorageAccountKey"]);
            CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, useHttps: true);

            try
            {
                //Imagenes de perfil: https://terecomiendoca.blob.core.windows.net/avatar/
                //Imagenes de convocatorias: https://terecomiendoca.blob.core.windows.net/logos/

                var nombreContenedor = contenedorImagen == ContenedoresImagenes.ImagenPerfil ? "avatar" : "logos";

                CloudBlobClient client = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference(nombreContenedor);

                container.CreateIfNotExists();

                container.SetPermissions(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });

                CloudBlockBlob blob = container.GetBlockBlobReference(usuarioId + "_original.jpg");
                blob.DeleteIfExists();

                blob = container.GetBlockBlobReference(usuarioId + "_thumb.jpg");
                blob.DeleteIfExists();
            }
            catch (Exception)
            {
                //
            }
        }

        public static string CopiarImagen(ContenedoresImagenes contenedorOrigen, ContenedoresImagenes contenedorDestino, string nombreImagenOrigen, string nombreImagenDestino)
        {
            StorageCredentials storageCredentials = new StorageCredentials(ConfigurationManager.AppSettings["ImagesStorageAccountName"], ConfigurationManager.AppSettings["ImagesStorageAccountKey"]);
            CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, useHttps: true);

            try
            {
                //Imagenes de perfil: https://terecomiendoca.blob.core.windows.net/avatar/
                //Imagenes de convocatorias: https://terecomiendoca.blob.core.windows.net/logos/

                var nombreContenedorOrigen = contenedorOrigen == ContenedoresImagenes.ImagenPerfil ? "avatar" : "logos";
                var nombreContenedorDestino = contenedorDestino == ContenedoresImagenes.ImagenPerfil ? "avatar" : "logos";

                CloudBlobClient client = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer containerOrigen = client.GetContainerReference(nombreContenedorOrigen);
                CloudBlobContainer containerDestino = client.GetContainerReference(nombreContenedorDestino);

                CloudBlockBlob blobOriginalOrigen = containerOrigen.GetBlockBlobReference(nombreImagenOrigen + "_original.jpg");

                if (blobOriginalOrigen.Exists())
                {
                    CloudBlockBlob blobOriginalDestino = containerDestino.GetBlockBlobReference(nombreImagenDestino + "_original.jpg");
                    blobOriginalDestino.DeleteIfExists();
                    blobOriginalDestino.StartCopy(blobOriginalOrigen);
                }

                CloudBlockBlob blobThumbnailOrigen = containerOrigen.GetBlockBlobReference(nombreImagenOrigen + "_thumb.jpg");
                if (blobThumbnailOrigen.Exists())
                {
                    CloudBlockBlob blobThumbnailDestino = containerDestino.GetBlockBlobReference(nombreImagenDestino + "_thumb.jpg");
                    blobThumbnailDestino.DeleteIfExists();
                    blobThumbnailDestino.StartCopy(blobThumbnailOrigen);
                }
            }
            catch (Exception)
            {
                //
            }

            return null;
        }

        #region Imagenes de perfil

        public static string ObtenerThumbnail(string usuarioId)
        {
            return $"{ConfigurationManager.AppSettings["ImagesStorageURL"]}{usuarioId}_thumb.jpg?{DateTime.UtcNow.Ticks}";
        }

        public static string ObtenerOriginal(string usuarioId)
        {
            return $"{ConfigurationManager.AppSettings["ImagesStorageURL"]}{usuarioId}_original.jpg?{DateTime.UtcNow.Ticks}";
        }

        public static string ObtenerOriginalDesdeThumbnail(string url)
        {
            return url.Replace("_thumb", "_original");
        }

        public static string ObtenerThumbnailPorDefecto()
        {
            return "/images/Perfil.png";
        }

        private static Image ResizeImage(Image imgToResize, Size size)
        {
            var sourceWidth = imgToResize.Width;
            var sourceHeight = imgToResize.Height;

            var nPercentW = size.Width / (float)sourceWidth;
            var nPercentH = size.Height / (float)sourceHeight;

            var nPercent = nPercentH < nPercentW ? nPercentH : nPercentW;

            var destWidth = (int)(sourceWidth * nPercent);
            var destHeight = (int)(sourceHeight * nPercent);

            var b = new Bitmap(destWidth, destHeight);
            var g = Graphics.FromImage(b);
            g.InterpolationMode = InterpolationMode.High;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return b;
        }

        #endregion

        #region Imagenes de convocatoriass

        public static string ObtenerLogoThumbnail(string logo)
        {
            return $"{ConfigurationManager.AppSettings["LogosStorageURL"]}{logo}_thumb.jpg?{DateTime.UtcNow.Ticks}";
        }

        public static string ObtenerLogoThumbnailPorDefecto(string area)
        {
            switch (area)
            {
                case "Administrativo":
                    return "/images/logosConvocatoria/Administrativo.svg";
                case "Agropecuario":
                    return "/images/logosConvocatoria/Agropecuario.svg";
                case "Belleza y estética":
                    return "/images/logosConvocatoria/BellezaEstetica.svg";
                case "Call center":
                    return "/images/logosConvocatoria/CallCenter.svg";
                case "Construcción":
                    return "/images/logosConvocatoria/Construccion.svg";
                case "Deportes y ejercicio":
                    return "/images/logosConvocatoria/DeportesEjercicio.svg";
                case "Diseño":
                    return "/images/logosConvocatoria/Diseno.svg";
                case "Domicilios y mensajería":
                    return "/images/logosConvocatoria/DomiciliosMensajeria.svg";
                case "Educación":
                    return "/images/logosConvocatoria/Educacion.svg";
                case "Hotelería y turismo":
                    return "/images/logosConvocatoria/HoteleriaTurismo.svg";
                case "Industria textil":
                    return "/images/logosConvocatoria/IndustriaTextil.svg";
                case "Industrial y manufacturas":
                    return "/images/logosConvocatoria/IndustrialManufacturas.svg";
                case "Limpieza":
                    return "/images/logosConvocatoria/Limpieza.svg";
                case "Logistica y transporte":
                    return "/images/logosConvocatoria/LogisticaTransporte.svg";
                case "Logística y transporte":
                    return "/images/logosConvocatoria/LogisticaTransporte.svg";
                case "Mecánica y talleres":
                    return "/images/logosConvocatoria/MecanicaTalleres.svg";
                case "Medios audiovisuales":
                    return "/images/logosConvocatoria/MediosAudiovisuales.svg";
                case "Restaurante y alimentos":
                    return "/images/logosConvocatoria/RestauranteAlimentos.svg";
                case "Salud":
                    return "/images/logosConvocatoria/Salud.svg";
                case "Seguridad industrial":
                    return "/images/logosConvocatoria/SeguridadIndustrial.svg";
                case "Servicios generales":
                    return "/images/logosConvocatoria/ServiciosGenerales.svg";
                case "Tecnología":
                    return "/images/logosConvocatoria/Tecnologia.svg";
                case "Ventas":
                    return "/images/logosConvocatoria/Ventas.svg";
                case "Vigilancia y seguridad":
                    return "/images/logosConvocatoria/VigilanciaSeguridad.svg";
                default:
                    return "/images/PerfilConvocatoria.png";
            }
        }

        public static string ObtenerImagenFacebookPorDefecto(string area)
        {
            switch (area)
            {
                case "Administrativo":
                    return "/images/logosConvocatoria/compartir/Administrativo.png";
                case "Agropecuario":
                    return "/images/logosConvocatoria/compartir/Agropecuario.png";
                case "Belleza y estética":
                    return "/images/logosConvocatoria/compartir/BellezaEstetica.png";
                case "Call center":
                    return "/images/logosConvocatoria/compartir/CallCenter.png";
                case "Construcción":
                    return "/images/logosConvocatoria/compartir/Construccion.png";
                case "Deportes y ejercicio":
                    return "/images/logosConvocatoria/compartir/DeportesEjercicio.png";
                case "Diseño":
                    return "/images/logosConvocatoria/compartir/Diseno.png";
                case "Domicilios y mensajería":
                    return "/images/logosConvocatoria/compartir/DomiciliosMensajeria.png";
                case "Educación":
                    return "/images/logosConvocatoria/compartir/Educacion.png";
                case "Hotelería y turismo":
                    return "/images/logosConvocatoria/compartir/HoteleriaTurismo.png";
                case "Industria textil":
                    return "/images/logosConvocatoria/compartir/IndustriaTextil.png";
                case "Industrial y manufacturas":
                    return "/images/logosConvocatoria/compartir/IndustrialManufacturas.png";
                case "Limpieza":
                    return "/images/logosConvocatoria/compartir/Limpieza.png";
                case "Logistica y transporte":
                    return "/images/logosConvocatoria/compartir/LogisticaTransporte.png";
                case "Logística y transporte":
                    return "/images/logosConvocatoria/compartir/LogisticaTransporte.png";
                case "Mecánica y talleres":
                    return "/images/logosConvocatoria/compartir/MecanicaTalleres.png";
                case "Medios audiovisuales":
                    return "/images/logosConvocatoria/compartir/MediosAudiovisuales.png";
                case "Restaurante y alimentos":
                    return "/images/logosConvocatoria/compartir/RestauranteAlimentos.png";
                case "Salud":
                    return "/images/logosConvocatoria/compartir/Salud.png";
                case "Seguridad industrial":
                    return "/images/logosConvocatoria/compartir/SeguridadIndustrial.png";
                case "Servicios generales":
                    return "/images/logosConvocatoria/compartir/ServiciosGenerales.png";
                case "Tecnología":
                    return "/images/logosConvocatoria/compartir/Tecnologia.png";
                case "Ventas":
                    return "/images/logosConvocatoria/compartir/Ventas.png";
                case "Vigilancia y seguridad":
                    return "/images/logosConvocatoria/compartir/VigilanciaSeguridad.png";
                default:
                    return "/images/logosConvocatoria/compartir/Convocatoria.png";
            }
        }

        #endregion

        #region Validación

        public static bool EsImagen(this HttpPostedFileBase httpPostedFileBase)
        {
            if (httpPostedFileBase == null)
                return false;

            // Valida los mime types

            if (httpPostedFileBase.ContentType.ToLower() != "image/jpg" && httpPostedFileBase.ContentType.ToLower() != "image/jpeg" &&
                httpPostedFileBase.ContentType.ToLower() != "image/pjpeg" && httpPostedFileBase.ContentType.ToLower() != "image/gif" &&
                httpPostedFileBase.ContentType.ToLower() != "image/x-png" && httpPostedFileBase.ContentType.ToLower() != "image/png")
            {
                return false;
            }

            // Valida la extensión

            if (httpPostedFileBase.FileName.IsNullOrWhiteSpace())
                return false;

            var extension = Path.GetExtension(httpPostedFileBase.FileName);

            if (extension.IsNullOrWhiteSpace())
                return false;

            if (extension != null && (extension.ToLower() != ".jpg" && extension.ToLower() != ".png" && extension.ToLower() != ".gif" && extension.ToLower() != ".jpeg"))
            {
                return false;
            }

            // Válida los primeros bytes

            try
            {
                if (!httpPostedFileBase.InputStream.CanRead)
                {
                    return false;
                }

                const int imageMinimumBytes = 512;
                if (httpPostedFileBase.ContentLength < imageMinimumBytes)
                {
                    return false;
                }

                byte[] buffer = new byte[512];
                httpPostedFileBase.InputStream.Read(buffer, 0, 512);
                string content = System.Text.Encoding.UTF8.GetString(buffer);

                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            // Trata de crear una nueva imagen, si lanza una excepción se asume que el archivo no es válido

            try
            {
                using (new Bitmap(httpPostedFileBase.InputStream))
                {
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                httpPostedFileBase.InputStream.Position = 0;
            }

            return true;
        }

        #endregion
    }
}
