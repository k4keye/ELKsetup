﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ELKsetup
{
    class ELKDownload
    {
        const int DOWNLOAD_COUNT = 4;
        string ELKversion = null;
        
        public int[] AllDownLoadStatus = new int[DOWNLOAD_COUNT];

        public async Task DownLoad(string version)
        {


            string[] DownLoadUri = new string[]{
            "https://artifacts.elastic.co/downloads/elasticsearch/elasticsearch-"+version+"-amd64.deb",
            "https://artifacts.elastic.co/downloads/logstash/logstash-"+version+".deb",
            "https://artifacts.elastic.co/downloads/kibana/kibana-"+version+"-amd64.deb",
            "https://artifacts.elastic.co/downloads/beats/filebeat/filebeat-"+version+"-amd64.deb"};

            for (byte i =0; i< DOWNLOAD_COUNT; i++)
            {
                string fileName = DownLoadUri[i].Split("/")[^1];
                string downloadPath = fileName;//실행경로에 다운받는다.

                ELKWebDownLoad(DownLoadUri[i], downloadPath,i);
            }
            DownLoadProgress elasticsearch_Progress = new DownLoadProgress();
            DownLoadProgress logstash_Progress = new DownLoadProgress();
            DownLoadProgress kibana_Progress = new DownLoadProgress();
            DownLoadProgress filebeat_Progress = new DownLoadProgress();
            string elasticsearch_Status, logstash_Status, kibana_Status, filebeat_Status;
            do
            {
                Thread.Sleep(1000);
                Console.Clear();
                elasticsearch_Status = elasticsearch_Progress.WriteProgressBar(AllDownLoadStatus[0]);
                logstash_Status= logstash_Progress.WriteProgressBar(AllDownLoadStatus[1]);
                kibana_Status= kibana_Progress.WriteProgressBar(AllDownLoadStatus[2]);
                filebeat_Status= filebeat_Progress.WriteProgressBar(AllDownLoadStatus[3]);

                Console.WriteLine(
                    $"[   Elastic   ] : {elasticsearch_Status}\n" +
                    $"[   Logstash  ] : {logstash_Status}\n" +
                    $"[    Kibana   ] : {kibana_Status}\n" +
                    $"[   Filebeat  ] : {filebeat_Status}\n");

                
            } while (!(AllDownLoadStatus[0] == 100 && AllDownLoadStatus[1] == 100 && AllDownLoadStatus[2] == 100 && AllDownLoadStatus[3] == 100));

                
        }

        private async void ELKWebDownLoad(string url ,string path, byte tag)
        {
            Uri uri = new Uri(url);
            try
            {                
                WebClient webClient = new WebClient();
                webClient.DownloadProgressChanged += (sender, e) => { WebClient_DownloadProgressChanged(sender, e, tag); };//무명 메소드로 이벤트로 매개변수 전달
                //webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged; 

                webClient.DownloadFileAsync(uri, path);
                webClient.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }            
        }
        
        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e, byte tag)
        {
            int thisStatus = e.ProgressPercentage;
            AllDownLoadStatus[tag] = thisStatus;

        }




    }
}
