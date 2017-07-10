﻿using Social.Domain.DomainServices;
using Social.Domain.Entities;
using Social.Infrastructure.Facebook;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Social.Infrastructure.Facebook.FacebookWebHookClient;

namespace Social.IntegrationTest
{
    public class FacebookServiceTest : TestBase
    {
        [Fact]
        public async Task ShouldProcessData_WhenNewMessageCreated()
        {
            string rawData = "{\"entry\": [{\"changes\": [{\"field\": \"conversations\", \"value\": {\"thread_id\": \"t_mid.$cAAdZrm4k4UZh9X1vd1bxDgkg7Bo9\", \"page_id\": 1974003879498745}}], \"id\": \"1974003879498745\", \"time\": 1498785109}], \"object\": \"page\"}";

            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<FbHookData>(rawData);
            IFacebookWebHookService facebookService = DependencyResolver.Resolve<IFacebookWebHookService>();
            await facebookService.ProcessWebHookData(data);
        }

        [Fact]
        public async Task ShouldProcessData_WhenVisitorPostCreated()
        {
            string rawData = "{\"entry\": [{\"changes\": [{\"field\": \"feed\", \"value\": {\"item\": \"post\", \"sender_name\": \"Vivi Xu\", \"sender_id\": \"121361878431739\", \"post_id\": \"1974003879498745_2015978795301253\", \"verb\": \"add\", \"created_time\": 1499675332, \"is_hidden\": false, \"message\": \"hi\"}}], \"id\": \"1974003879498745\", \"time\": 1499675333}], \"object\": \"page\"}";

            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<FbHookData>(rawData);
            IFacebookWebHookService facebookService = DependencyResolver.Resolve<IFacebookWebHookService>();
            await facebookService.ProcessWebHookData(data);
        }

        [Fact]
        public async Task ShouldProcessData_WhenVisitorPostRemoved()
        {
            string rawData = "{ \"entry\": [{\"changes\": [{\"field\": \"feed\", \"value\": {\"verb\": \"remove\", \"item\": \"post\", \"sender_name\": \"Vivi Xu\", \"sender_id\": \"121361878431739\", \"post_id\": \"1974003879498745_2015978795301253\", \"recipient_id\": \"1974003879498745\", \"created_time\": 1499134045}}], \"id\": \"1974003879498745\", \"time\": 1499678628}], \"object\": \"page\"}";

            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<FbHookData>(rawData);
            IFacebookWebHookService facebookService = DependencyResolver.Resolve<IFacebookWebHookService>();
            await facebookService.ProcessWebHookData(data);
        }

        [Fact]
        public async Task ShouldProcessData_WhenNewPhotoVisitorPost()
        {
            string rawData = "{\"entry\": [{\"changes\": [{\"field\": \"feed\", \"value\": {\"photo_id\": \"152819685285958\", \"item\": \"photo\", \"sender_name\": \"Vivi Xu\", \"sender_id\": \"121361878431739\", \"post_id\": \"1974003879498745_2015985455300587\", \"verb\": \"add\", \"link\": \"https://scontent.xx.fbcdn.net/v/t31.0-8/19956231_152819685285958_6092590179113745862_o.jpg?oh=f941d75af589ab07e2121cc9f3856efd&oe=59C91EB0\", \"published\": 1, \"created_time\": 1499680930, \"message\": \"this is a picture\"}}], \"id\": \"1974003879498745\", \"time\": 1499680949}], \"object\": \"page\"}";

            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<FbHookData>(rawData);
            IFacebookWebHookService facebookService = DependencyResolver.Resolve<IFacebookWebHookService>();
            await facebookService.ProcessWebHookData(data);
        }


        //[Fact]
        //public async Task ShouldProcessData_WhenPostCommentCreated()
        //{

        //}
    }
}