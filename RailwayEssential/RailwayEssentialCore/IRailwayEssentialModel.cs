﻿using System.Collections.Generic;
using System.Windows.Input;

namespace RailwayEssentialCore
{
    public interface IRailwayEssentialModel
    {
        bool IsVisualLabelActivated { get; set; }

        void TriggerPropertyChanged(string name);
        void SetCurrentLocomotive(object locomotiveItem);
        void SetCurrentSwitch(object switchItem);
        void SetDirty(bool state);
        void ShowBlockRoutePreview(object blockRouteItem);
        void ResetBlockRoutePreview();
    }
}
