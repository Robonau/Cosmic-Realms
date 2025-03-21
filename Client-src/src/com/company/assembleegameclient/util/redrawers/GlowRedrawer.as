﻿package com.company.assembleegameclient.util.redrawers {
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.util.TextureRedrawer;
import com.company.util.PointUtil;

import flash.display.Bitmap;
import flash.display.BitmapData;
import flash.display.BlendMode;
import flash.display.GradientType;
import flash.display.Shape;
import flash.filters.BitmapFilterQuality;
import flash.filters.GlowFilter;
import flash.geom.Matrix;
import flash.utils.Dictionary;

public class GlowRedrawer {

    private static const GRADIENT_MAX_SUB:uint = 0x282828;


    private static var tempMatrix_:Matrix = new Matrix();
    private static var gradient_:Shape = getGradient();
    private static var glowHashes:Dictionary = new Dictionary();


    public static function outlineGlow(_arg1:BitmapData, _arg2:uint, _arg3:Number = 1.4, _arg4:Boolean = true, itemGlowStrenght:Number = 3, outlineGlow = false):BitmapData {
        var GLOW_FILTER_ALT:GlowFilter = new GlowFilter(0, 0.5, 16, 16, itemGlowStrenght, BitmapFilterQuality.LOW, false, false);
        var GLOW_FILTER_OUT:GlowFilter = new GlowFilter(0, 0, 0, 0, 0, 0, false, false);
        if (Parameters.data_.SuperGlow) {
            GLOW_FILTER_ALT = new GlowFilter(0, 0.8, 3, 3, 255, 1, false, false);
            GLOW_FILTER_OUT = new GlowFilter(0, 0.2, 5, 5, 5, 1, false, false);

        }

        var _local5:String = getHash(_arg2, _arg3);
        if (((_arg4) && (isCached(_arg1, _local5)))) {
            return (glowHashes[_arg1][_local5]);
        }
        var _local6:BitmapData = _arg1.clone();
        tempMatrix_.identity();
        tempMatrix_.scale((_arg1.width / 256), (_arg1.height / 256));
        _local6.draw(gradient_, tempMatrix_, null, BlendMode.SUBTRACT);
        var _local7:Bitmap = new Bitmap(_arg1);
        _local6.draw(_local7, null, null, BlendMode.ALPHA);
        TextureRedrawer.OUTLINE_FILTER.blurX = _arg3;
        TextureRedrawer.OUTLINE_FILTER.blurY = _arg3;
        var _local8:uint;
        TextureRedrawer.OUTLINE_FILTER.color = _local8;
        _local6.applyFilter(_local6, _local6.rect, PointUtil.ORIGIN, TextureRedrawer.OUTLINE_FILTER);
        if (_arg2 != 4294967295) {
            if (_arg2 != 0) {
                GLOW_FILTER_ALT.color = _arg2;
                GLOW_FILTER_OUT.color = _arg2;
                _local6.applyFilter(_local6, _local6.rect, PointUtil.ORIGIN, GLOW_FILTER_ALT);
                _local6.applyFilter(_local6, _local6.rect, PointUtil.ORIGIN, GLOW_FILTER_OUT);
            }
        }
        if (_arg4) {
            cache(_arg1, _arg2, _arg3, _local6);
        }
        return (_local6);
    }

    private static function cache(_arg1:BitmapData, _arg2:uint, _arg3:Number, _arg4:BitmapData):void {
        var _local6:Object;
        var _local5:String = getHash(_arg2, _arg3);
        if ((_arg1 in glowHashes)) {
            glowHashes[_arg1][_local5] = _arg4;
        }
        else {
            _local6 = {};
            _local6[_local5] = _arg4;
            glowHashes[_arg1] = _local6;
        }
    }

    private static function isCached(_arg1:BitmapData, _arg2:String):Boolean {
        var _local3:Object;
        if ((_arg1 in glowHashes)) {
            _local3 = glowHashes[_arg1];
            if ((_arg2 in _local3)) {
                return true;
            }
        }
        return false;
    }

    private static function getHash(_arg1:uint, _arg2:Number):String {
        return ((int((_arg2 * 10)).toString() + _arg1));
    }

    private static function getGradient():Shape {
        var _local1:Shape = new Shape();
        var _local2:Matrix = new Matrix();
        _local2.createGradientBox(256, 256, (Math.PI / 2), 0, 0);
        _local1.graphics.beginGradientFill(GradientType.LINEAR, [0, GRADIENT_MAX_SUB], [1, 1], [127, 255], _local2);
        _local1.graphics.drawRect(0, 0, 256, 256);
        _local1.graphics.endFill();
        return (_local1);
    }

    public static function clearCache():void
    {
        for each (var dict:Dictionary in glowHashes) {
            for each (var tex:BitmapData in dict) {
                if (tex != null)
                {
                    tex.dispose();
                }
            }
        }
        glowHashes = new Dictionary();
    }


}
}
