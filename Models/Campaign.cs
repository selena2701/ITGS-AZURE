using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Models
{
    public class Campaign
    {
        private string campaignId;
        private string campaignName;
        private string campaignImage;
        private DateTime dateStart;
        private DateTime dateFinish;
        private int status;
        private string campaignContent, campaignNote;

        public Campaign()
        {
        }

        public Campaign(string campaignId, string campaignName, string campaignImage, DateTime dateStart, DateTime dateFinish, int status, string campaignContent, string campaignNote)
        {
            this.campaignId = campaignId;
            this.campaignName = campaignName;
            this.campaignImage = campaignImage;
            this.dateStart = dateStart;
            this.dateFinish = dateFinish;
            this.status = status;
            this.campaignContent = campaignContent;
            this.campaignNote = campaignNote;
        }

        public string CampaignId { get => campaignId; set => campaignId = value; }
        public string CampaignName { get => campaignName; set => campaignName = value; }
        public string CampaignImage { get => campaignImage; set => campaignImage = value; }
        public DateTime DateStart { get => dateStart; set => dateStart = value; }
        public DateTime DateFinish { get => dateFinish; set => dateFinish = value; }
        public int Status { get => status; set => status = value; }
        public string CampaignContent { get => campaignContent; set => campaignContent = value; }
        public string CampaignNote { get => campaignNote; set => campaignNote = value; }
    }
}
