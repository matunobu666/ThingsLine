using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IdentityModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;





namespace ThingsLine.Modules
{
    public class modStorage
    {
//        private static readonly string connectionString = ConfigurationManager.AppSettings.Get("DefaultConnection");
//        private static readonly string accountName = ConfigurationManager.AppSettings.Get("accountName");
//        private static readonly string accessKey = ConfigurationManager.AppSettings.Get("accessKey");


        private static readonly string connectionString = "DefaultEndpointsProtocol=https;AccountName=matu666;AccountKey=MzPChTfZXCUxpgGLJIDYK6Vvx0aEFXcaX76NUwGfJsguWLoaA8aZqyVRJad5jYxJIOahb7znjumvCJsntcNf7A==;EndpointSuffix=core.windows.net";
        private static readonly string accountName = "matu666";
        private static readonly string accessKey = "MzPChTfZXCUxpgGLJIDYK6Vvx0aEFXcaX76NUwGfJsguWLoaA8aZqyVRJad5jYxJIOahb7znjumvCJsntcNf7A==";


        //-----------------------------------
        //BLOB間でのファイル移動
        //-------------------------------    
        public async Task CopyBlobAsync(string FromBlobContainer, string ToBlobContainer, string blobName)
        {

            // コンテナへの参照を取得し、それを作成します
            BlobContainerClient FromContainer = new BlobContainerClient(connectionString, FromBlobContainer);
            // コピー元BLOBを表すBlobClientを作成します。
            BlobClient FromBlob = FromContainer.GetBlobClient(blobName);

            // コンテナへの参照を取得し、それを作成します
            BlobContainerClient ToContainer = new BlobContainerClient(connectionString, ToBlobContainer);
            //コンテナもし無かったら作る
            ToContainer.CreateIfNotExists(PublicAccessType.BlobContainer);
            // コピー先BlobClientを取得します。
            BlobClient ToBlob = ToContainer.GetBlobClient(blobName);

            try
            {


                // ソースBLOBが存在することを確認してください。
                if (await FromBlob.ExistsAsync())
                {
                    // コピー操作のためにソースBLOBをリースします
                    //別のクライアントが変更できないようにします。
                    BlobLeaseClient lease = FromBlob.GetBlobLeaseClient();

                    // リース間隔に-1を指定すると、無限リースが作成されます。
                    await lease.AcquireAsync(TimeSpan.FromSeconds(-1));

                    // ソースblobのプロパティを取得し、リース状態を表示します。
                    Azure.Storage.Blobs.Models.BlobProperties sourceProperties = await FromBlob.GetPropertiesAsync();
                    Console.WriteLine($"Lease state: {sourceProperties.LeaseState}");

                    // コピー操作を開始します。
                    await ToBlob.StartCopyFromUriAsync(FromBlob.Uri);

                    // 宛先blobのプロパティを取得し、コピーステータスを表示します。
                    Azure.Storage.Blobs.Models.BlobProperties destProperties = await ToBlob.GetPropertiesAsync();

                    // ソースblobのプロパティを更新します。
                    sourceProperties = await FromBlob.GetPropertiesAsync();

                    if (sourceProperties.LeaseState == Azure.Storage.Blobs.Models.LeaseState.Leased)
                    {
                        // ソースBLOBのリースを解除します。
                        await lease.BreakAsync();

                        // ソースblobのプロパティを更新して、リース状態を確認します。
                        sourceProperties = await FromBlob.GetPropertiesAsync();
                        Console.WriteLine($"Lease state: {sourceProperties.LeaseState}");

                        //削除処理
                        await FromBlob.DeleteAsync();
                    }
                }
            }
            catch (Azure.RequestFailedException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                throw;
            }
        }


        //-----------------------------------
        //画像の保存
        //-------------------------------        
        public bool BlobSave(Stream response, string getFolderName, string getFileName)
        {


            //storageAccountの作成（接続情報の定義）
            var credential = new StorageCredentials(accountName, accessKey);
            var storageAccount = new CloudStorageAccount(credential, true);

            //blob
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            //container
            CloudBlobContainer container = blobClient.GetContainerReference(getFolderName);

            //アップロード後のファイル名を指定
            CloudBlockBlob blockBlob_upload = container.GetBlockBlobReference(getFileName);


            //アップロード処理
            //アップロードしたいローカルのファイルを指定

            var attachment = blockBlob_upload.UploadFromStreamAsync(response);

            return true;
        }



    }
}
