﻿using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace TDMakerLib
{
    public static class TorrentInfo
    {
        public static string ToStringPublishExternal(PublishOptions options, TemplateReader tr)
        {
            tr.CreateInfo(options);

            return BbFormat(tr.PublishInfo, options);
        }

        public static string ToStringPublishMediaInfo(TaskSettings ts)
        {
            StringBuilder sbPublish = new StringBuilder();

            switch (ts.MediaOptions.MediaTypeChoice)
            {
                case MediaType.MediaDisc:
                    StringBuilder sbMediaInfo = new StringBuilder();
                    if (ts.Media.MediaFiles.Count > 0)
                    {
                        foreach (MediaFile mf in ts.Media.MediaFiles)
                        {
                            sbMediaInfo.AppendLine(BbCode.Bold(mf.FileName));
                            sbMediaInfo.AppendLine(mf.Summary.Trim());
                            sbMediaInfo.AppendLine();
                        }
                    }
                    else
                    {
                        sbMediaInfo.AppendLine(ts.Media.Overall.Summary.Trim());
                        sbMediaInfo.AppendLine();
                    }

                    sbPublish.AppendLine(BbFormat(sbMediaInfo.ToString(), ts.PublishOptions));

                    if (ts.MediaOptions.UploadScreenshots)
                        sbPublish.AppendLine(ts.Media.Overall.GetScreenshotString(ts.PublishOptions));

                    break;
                default:
                    foreach (MediaFile mf in ts.Media.MediaFiles)
                    {
                        sbMediaInfo = new StringBuilder();
                        sbMediaInfo.AppendLine(mf.Summary.Trim());
                        sbMediaInfo.AppendLine();

                        sbPublish.AppendLine(BbFormat(sbMediaInfo.ToString(), ts.PublishOptions));

                        if (ts.MediaOptions.UploadScreenshots)
                        {
                            sbPublish.AppendLine();
                            sbPublish.AppendLine(mf.GetScreenshotString(ts.PublishOptions));
                        }
                    }

                    break;
            }

            string publishInfo = sbPublish.ToString().Trim();

            if (App.Settings.ProfileActive.HidePrivateInfo)
            {
                publishInfo = Regex.Replace(publishInfo, "(?<=Complete name *: ).+?(?=\\r)", match => Path.GetFileName(match.Value));
            }

            return publishInfo;
        }

        public static string ToStringPublishInternal(TaskSettings ts)
        {
            StringBuilder sbPublish = new StringBuilder();
            string info = ts.MediaOptions.MediaTypeChoice == MediaType.MusicAudioAlbum ? ts.Media.ToStringAudio() : ts.ToStringMedia();
            sbPublish.Append(BbFormat(info, ts.PublishOptions));

            return sbPublish.ToString().Trim();
        }

        private static string BbFormat(string p, PublishOptions options)
        {
            StringBuilder sbPublish = new StringBuilder();

            if (options.AlignCenter)
            {
                p = BbCode.AlignCenter(p);
            }

            if (options.PreformattedText)
            {
                sbPublish.AppendLine(BbCode.Pre(p));
            }
            else
            {
                sbPublish.AppendLine(p);
            }

            return sbPublish.ToString().Trim();
        }
    }
}