﻿using BarRaider.SdTools;
using System;

namespace BarRaider.GPMDP.Actions
{
    [PluginActionId("com.barraider.gpmdp.volup")]
    public class VolumeUpAction : VolumeActionBase
    {
        public VolumeUpAction(SDConnection connection, InitialPayload payload) : base(connection, payload)
        {
        }

        public async override void KeyPressed(KeyPayload payload)
        {
            baseHandledKeypress = false;
            base.KeyPressed(payload);

            if (!baseHandledKeypress)
            {
                if (!gpmdpManager.IsConnected)
                {
                    await Connection.ShowAlert();
                    return;
                }

                if (int.TryParse(Settings.VolumeParam, out int volume))
                {
                    // Spotify gets fussy if the values are out of range
                    int totalVolume = gpmdpManager.GetVolume() + volume;
                    if (totalVolume > 100)
                    {
                        totalVolume = 100;
                    }
                    gpmdpManager.SetVolume(totalVolume);
                }
            }
        }

        public async override void OnTick()
        {
            baseHandledOnTick = false;
            base.OnTick();

            if (!baseHandledOnTick)
            {
                await Connection.SetImageAsync((String)null);

                if (Settings.ShowVolumeLevel && gpmdpManager.IsConnected)
                {
                    await Connection.SetTitleAsync(gpmdpManager.GetVolume().ToString());
                }
                else
                {
                    await Connection.SetTitleAsync(null);
                }
            }
        }
    }
}
