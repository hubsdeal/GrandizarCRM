using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.LookupData.Exporting;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using SoftGrid.Storage;
using SoftGrid.UtilityDtos;
using System.Net.Http.Headers;
using System.Web;
using Newtonsoft.Json;
using IdentityModel;
using NPOI.POIFS.Crypt;
using MailKit.Net.Smtp;
using Unifonic.NetCore.Models;
using MimeKit;
using Org.BouncyCastle.Cms;
using Unifonic.NetCore.Controllers;
using Unifonic.NetCore.Exceptions;

namespace SoftGrid.LookupData
{
    [AbpAuthorize(AppPermissions.Pages_SmsTemplates)]
    public class SmsTemplatesAppService : SoftGridAppServiceBase, ISmsTemplatesAppService
    {
        private readonly IRepository<SmsTemplate, long> _smsTemplateRepository;
        private readonly ISmsTemplatesExcelExporter _smsTemplatesExcelExporter;

        public SmsTemplatesAppService(IRepository<SmsTemplate, long> smsTemplateRepository, ISmsTemplatesExcelExporter smsTemplatesExcelExporter)
        {
            _smsTemplateRepository = smsTemplateRepository;
            _smsTemplatesExcelExporter = smsTemplatesExcelExporter;

        }

        public async Task<PagedResultDto<GetSmsTemplateForViewDto>> GetAll(GetAllSmsTemplatesInput input)
        {

            var filteredSmsTemplates = _smsTemplateRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Title.Contains(input.Filter) || e.Content.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title.Contains(input.TitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContentFilter), e => e.Content.Contains(input.ContentFilter))
                        .WhereIf(input.PublishedFilter.HasValue && input.PublishedFilter > -1, e => (input.PublishedFilter == 1 && e.Published) || (input.PublishedFilter == 0 && !e.Published));

            var pagedAndFilteredSmsTemplates = filteredSmsTemplates
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var smsTemplates = from o in pagedAndFilteredSmsTemplates
                               select new
                               {

                                   o.Title,
                                   o.Content,
                                   o.Published,
                                   Id = o.Id
                               };

            var totalCount = await filteredSmsTemplates.CountAsync();

            var dbList = await smsTemplates.ToListAsync();
            var results = new List<GetSmsTemplateForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetSmsTemplateForViewDto()
                {
                    SmsTemplate = new SmsTemplateDto
                    {

                        Title = o.Title,
                        Content = o.Content,
                        Published = o.Published,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetSmsTemplateForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetSmsTemplateForViewDto> GetSmsTemplateForView(long id)
        {
            var smsTemplate = await _smsTemplateRepository.GetAsync(id);

            var output = new GetSmsTemplateForViewDto { SmsTemplate = ObjectMapper.Map<SmsTemplateDto>(smsTemplate) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_SmsTemplates_Edit)]
        public async Task<GetSmsTemplateForEditOutput> GetSmsTemplateForEdit(EntityDto<long> input)
        {
            var smsTemplate = await _smsTemplateRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetSmsTemplateForEditOutput { SmsTemplate = ObjectMapper.Map<CreateOrEditSmsTemplateDto>(smsTemplate) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditSmsTemplateDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_SmsTemplates_Create)]
        protected virtual async Task Create(CreateOrEditSmsTemplateDto input)
        {
            var smsTemplate = ObjectMapper.Map<SmsTemplate>(input);

            if (AbpSession.TenantId != null)
            {
                smsTemplate.TenantId = (int?)AbpSession.TenantId;
            }

            await _smsTemplateRepository.InsertAsync(smsTemplate);

        }

        [AbpAuthorize(AppPermissions.Pages_SmsTemplates_Edit)]
        protected virtual async Task Update(CreateOrEditSmsTemplateDto input)
        {
            var smsTemplate = await _smsTemplateRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, smsTemplate);

        }

        [AbpAuthorize(AppPermissions.Pages_SmsTemplates_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _smsTemplateRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetSmsTemplatesToExcel(GetAllSmsTemplatesForExcelInput input)
        {

            var filteredSmsTemplates = _smsTemplateRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Title.Contains(input.Filter) || e.Content.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title.Contains(input.TitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContentFilter), e => e.Content.Contains(input.ContentFilter))
                        .WhereIf(input.PublishedFilter.HasValue && input.PublishedFilter > -1, e => (input.PublishedFilter == 1 && e.Published) || (input.PublishedFilter == 0 && !e.Published));

            var query = (from o in filteredSmsTemplates
                         select new GetSmsTemplateForViewDto()
                         {
                             SmsTemplate = new SmsTemplateDto
                             {
                                 Title = o.Title,
                                 Content = o.Content,
                                 Published = o.Published,
                                 Id = o.Id
                             }
                         });

            var smsTemplateListDtos = await query.ToListAsync();

            return _smsTemplatesExcelExporter.ExportToFile(smsTemplateListDtos);
        }

        [AbpAllowAnonymous]
        [HttpGet("SendSms")]
        public async Task<SmSRequestDto> SendSmsAsync(SmSRequestDto dtoModel)
        {
            try
            {
                if (dtoModel == null) throw new Exception("Sorry! No Data Found to Send SMS!");
                if (dtoModel.Body == null) throw new Exception("Sorry! No SMS Body Found to Send SMS!");

                var appsId = "4cLvMBEmZqzwTMCANNYAsmqYA4jAWr";
                var baseUrl = $"https://el.cloud.unifonic.com/rest/SMS/messages?AppSid={appsId}&SenderId=sender&";
                var correlationID = DateTime.Now.Ticks.ToString();
                //Unifonic.NetCore.Configuration.BasicAuthUserName = "";
                //Unifonic.NetCore.Configuration.BasicAuthPassword = "";
                // Unifonic.NetCore.UnifonicNextGenClient smsClient = new Unifonic.NetCore.UnifonicNextGenClient();
                // SendResponse resultA = await smsClient.Rest.CreateSendMessageAsync(appsId, "", dtoModel.Body, dtoModel.Recipient);




                var client1 = new Unifonic.NetCore.UnifonicNextGenClient();
                RestController rest = client1.Rest;

                string senderID = "sender";
                string responseType = "JSON";
                bool? baseEncode = true;
                string statusCallback = "sent";
                bool? masync = false;


                SendResponse result1 = rest.CreateSendMessageAsync(appsId, senderID, dtoModel.Body, dtoModel.Recipient, responseType, correlationID, baseEncode, statusCallback, masync).Result;






                using var client = GethttpClient();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                dtoModel.Body = dtoModel.Body.Replace("\r\n", "NEWLINE");
                var urlEncode = HttpUtility.UrlEncode(dtoModel.Body);
                urlEncode = urlEncode.Replace("NEWLINE", "\n");
                var jsonSerialize = JsonConvert.SerializeObject(new { appSid = appsId, recipient = dtoModel.Recipient, body = urlEncode, statusCallback = "", async = false, correlationId = DateTime.Now.Ticks });


                //for testing purpose
                //'https://el.cloud.unifonic.com/rest/SMS/messages?AppSid=axuN0U7QlmqVPsfdgoK0mZFgdzG16p&SenderID=UNISMS&Body=Test
                //message&Recipient=971507679351&responseType=JSON&CorrelationID=q1&baseEncode=true&statusCallback=sent&async=false'



                var finalUrl = $"{baseUrl}&Body={jsonSerialize}";
                var httpResponse = await client.GetAsync(finalUrl);
                var responseString = await httpResponse.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(responseString);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }


        //[HttpGet("")]
        public async Task<SmSRequestDto> SendOtp(SmSRequestDto dtoModel)
        {
            if (dtoModel == null) throw new Exception("Sorry! No Data Found to Send SMS!");
            if (dtoModel.Recipient == null) throw new Exception("Sorry! No Recipient Found to Send SMS!");

            using var client = GethttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            dtoModel.Body = dtoModel.Body.Replace("\r\n", "NEWLINE");
            var urlEncode = HttpUtility.UrlEncode(dtoModel.Body);
            urlEncode = urlEncode.Replace("NEWLINE", "\n");
            var jsonSerialize = JsonConvert.SerializeObject(new { body = urlEncode });


            //for testing purpose
            //'https://el.cloud.unifonic.com/rest/SMS/messages?AppSid=axuN0U7QlmqVPsfdgoK0mZFgdzG16p&SenderID=UNISMS&Body=Test
            //message&Recipient=971507679351&responseType=JSON&CorrelationID=q1&baseEncode=true&statusCallback=sent&async=false'

            var baseUrl = "https://el.cloud.unifonic.com/rest/SMS/messages?AppSid=axuN0U7QlmqVPsfdgoK0mZFgdzG16p&SenderID=UNISMS&";

            var finalUrl = $"{baseUrl}&Body={jsonSerialize}";
            var httpResponse = await client.GetAsync(finalUrl);
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<dynamic>(responseString);

            //store OTP in database/ Cache here

            return result;
        }



        public async Task<bool> OtpVerify(string mobileNumber, string otp)
        {
            //get OTP from database/ Cache here
            var otpFromDb = "1234";
            if (otpFromDb == otp)
            {
                return true;
            }
            return false;

        }


        private HttpClient GethttpClient()
        {
            var clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            var client = new HttpClient(clientHandler);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }


        //private async Task<bool> SendSms()
        //{
        //    Unifonic.NetCore.Configuration.BasicAuthUserName = "yourUsername";
        //    Unifonic.NetCore.Configuration.BasicAuthPassword = "yourPassword";
        //    Unifonic.NetCore.UnifonicNextGenClient smsClient = new Unifonic.NetCore.UnifonicNextGenClient();
        //    string appSid = "appSid";
        //    string senderID = "senderID";
        //    long recipient = long.Parse("+0123456789");
        //    //Asynchronous
        //    SendResponse resultA = await smsClient.Rest.CreateSendMessageAsync(appSid, senderID, message, recipient);
        //    //Synchronous
        //    SendResponse resultS = smsClient.Rest.CreateSendMessage(appSid, senderID, message, recipient);
        //}

    }
}