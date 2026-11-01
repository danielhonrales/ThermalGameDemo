// Draws the game's virtual content, rendered onto a transparent background, over the passthrough
// camera picture for the spectator's player view (PlayerViewSender). The content is treated as
// premultiplied: opaque pixels replace the camera picture, additive effects add to it.
Shader "Hidden/ThermalGame/PlayerViewComposite"
{
    Properties
    {
        _MainTex ("Virtual content", 2D) = "black" {}
    }
    SubShader
    {
        ZTest Always Cull Off ZWrite Off
        Blend One OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;

            fixed4 frag(v2f_img i) : SV_Target
            {
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}
