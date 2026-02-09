Shader "Custom/ScreenCircleInvert"
{
    Properties
    {
        _MainTex ("Screen", 2D) = "white" {}
        _Radius ("Radius", Float) = 0.5
        _SoftEdge ("Soft Edge", Float) = 0.02
    }

    SubShader
    {
        Tags { "Queue"="Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _Radius;
            float _SoftEdge;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 center = float2(0.5, 0.5);
                float dist = distance(i.uv, center);
                float mask = smoothstep(_Radius, _Radius - _SoftEdge, dist);

                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb = lerp(col.rgb, 1.0 - col.rgb, mask);
                col.a = mask;

                return col;
            }
            ENDCG
        }
    }
}
