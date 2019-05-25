Shader "Custom/ShadowStencil"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque"
               "Queue"="Transparent"
               "IgnoreProjector"="True" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull off

        // ステンシルバッファが1の場合は黒に塗りつぶす
        Pass
        {
            Stencil
            {
                Ref 1
                Comp Equal
            }
            
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            sampler2D _MainTex;
            
            fixed4 frag(v2f_img i) : SV_Target
            {
                float alpha = tex2D(_MainTex, i.uv).a;
                fixed4 col = fixed4(0, 0, 0, alpha);
                return col;
            }
            ENDCG
        }
        
        // ステンシルバッファが0の場合はそのまま描画
        Pass
        {
            Stencil
            {
                Ref 0
                Comp Equal
            }
            
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            sampler2D _MainTex;
            
            fixed4 frag(v2f_img i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}