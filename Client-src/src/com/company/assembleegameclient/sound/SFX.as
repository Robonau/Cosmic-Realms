﻿package com.company.assembleegameclient.sound {
import com.company.assembleegameclient.parameters.Parameters;

import flash.media.SoundTransform;

public class SFX {

    private static var sfxTrans_:SoundTransform;


    public static function load():void {
        sfxTrans_ = new SoundTransform(((Parameters.data_.playSFX) ? 1 : 0));
        SoundEffectLibrary.updateVolume(100 / 4);
    }

    public static function setPlaySFX(_arg1:Boolean):void {
        Parameters.data_.playSFX = _arg1;
        Parameters.save();
        SoundEffectLibrary.updateTransform();
        SoundEffectLibrary.updateVolume(100 / 4);
    }

    public static function setSFXVolume(_arg1:Number):void {
        Parameters.data_.SFXVolume = _arg1;
        Parameters.save();
        SoundEffectLibrary.updateVolume(_arg1 / 4);
    }


}
}
