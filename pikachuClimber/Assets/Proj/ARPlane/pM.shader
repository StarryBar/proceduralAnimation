Shader "Custom/pM"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TexTintColor("Texture Tint Color", Color) = (1,1,1,1)
        _PlaneColor("Plane Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline" = "UniversalPipeline"}
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 uv2 : TEXCOORD1;
            };
            struct Varyings
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 uv2 : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            half4 _TexTintColor;
            half4 _PlaneColor;
            float _ShortestUVMapping;



            Varyings vert(Attributes v)
            {
                Varyings o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv2 = v.uv2;
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                half4 col = tex2D(_MainTex, i.uv) * _TexTintColor;
                col = lerp( _PlaneColor, col, col.a);
                // Fade out from as we pass the edge.
                // uv2.x stores a mapped UV that will be "1" at the beginning of the feathering.
                // We fade until we reach at the edge of the shortest UV mapping.
                // This is the remmaped UV value at the vertex.
                // We choose the shorted one so that ll edges will fade out completely.
                // See ARFeatheredPlaneMeshVisualizer.cs for more details.
                col.a *= 1-smoothstep(1, _ShortestUVMapping, i.uv2.x);
                return col;
            }
            ENDHLSL
        }
    }
    
}
