Shader "Unlit/EvolvingShader"
{
    Properties
    {
        _ScalingFactor("Scaling Factor", Float) = 1.0
        _OperatorCountR("Operator Count R", Int) = 0
        _OperatorsR0("Operators R 0", Vector) = (0, 0, 0, 0)
        _OperatorsR1("Operators R 1", Vector) = (0, 0, 0, 0)
        _OperatorsR2("Operators R 2", Vector) = (0, 0, 0, 0)
        _OperatorsR3("Operators R 3", Vector) = (0, 0, 0, 0)
        _OperatorsR4("Operators R 4", Vector) = (0, 0, 0, 0)
        _OperatorsR5("Operators R 5", Vector) = (0, 0, 0, 0)
        _OperatorsR6("Operators R 6", Vector) = (0, 0, 0, 0)
        _OperatorsR7("Operators R 7", Vector) = (0, 0, 0, 0)
        _OperatorsR8("Operators R 8", Vector) = (0, 0, 0, 0)
        _OperatorsR9("Operators R 9", Vector) = (0, 0, 0, 0)
        _OperatorsR10("Operators R 10", Vector) = (0, 0, 0, 0)
        _OperatorsR11("Operators R 11", Vector) = (0, 0, 0, 0)
        _OperatorsR12("Operators R 12", Vector) = (0, 0, 0, 0)
        _OperatorsR13("Operators R 13", Vector) = (0, 0, 0, 0)
        _OperatorsR14("Operators R 14", Vector) = (0, 0, 0, 0)
        _OperatorsR15("Operators R 15", Vector) = (0, 0, 0, 0)

        _OperatorCountG("Operator Count G", Int) = 0
        _OperatorsG0("Operators G 0", Vector) = (0, 0, 0, 0)
        _OperatorsG1("Operators G 1", Vector) = (0, 0, 0, 0)
        _OperatorsG2("Operators G 2", Vector) = (0, 0, 0, 0)
        _OperatorsG3("Operators G 3", Vector) = (0, 0, 0, 0)
        _OperatorsG4("Operators G 4", Vector) = (0, 0, 0, 0)
        _OperatorsG5("Operators G 5", Vector) = (0, 0, 0, 0)
        _OperatorsG6("Operators G 6", Vector) = (0, 0, 0, 0)
        _OperatorsG7("Operators G 7", Vector) = (0, 0, 0, 0)
        _OperatorsG8("Operators G 8", Vector) = (0, 0, 0, 0)
        _OperatorsG9("Operators G 9", Vector) = (0, 0, 0, 0)
        _OperatorsG10("Operators G 10", Vector) = (0, 0, 0, 0)
        _OperatorsG11("Operators G 11", Vector) = (0, 0, 0, 0)
        _OperatorsG12("Operators G 12", Vector) = (0, 0, 0, 0)
        _OperatorsG13("Operators G 13", Vector) = (0, 0, 0, 0)
        _OperatorsG14("Operators G 14", Vector) = (0, 0, 0, 0)
        _OperatorsG15("Operators G 15", Vector) = (0, 0, 0, 0)

        _OperatorCountB("Operator Count B", Int) = 0
        _OperatorsB0("Operators B 0", Vector) = (0, 0, 0, 0)
        _OperatorsB1("Operators B 1", Vector) = (0, 0, 0, 0)
        _OperatorsB2("Operators B 2", Vector) = (0, 0, 0, 0)
        _OperatorsB3("Operators B 3", Vector) = (0, 0, 0, 0)
        _OperatorsB4("Operators B 4", Vector) = (0, 0, 0, 0)
        _OperatorsB5("Operators B 5", Vector) = (0, 0, 0, 0)
        _OperatorsB6("Operators B 6", Vector) = (0, 0, 0, 0)
        _OperatorsB7("Operators B 7", Vector) = (0, 0, 0, 0)
        _OperatorsB8("Operators B 8", Vector) = (0, 0, 0, 0)
        _OperatorsB9("Operators B 9", Vector) = (0, 0, 0, 0)
        _OperatorsB10("Operators B 10", Vector) = (0, 0, 0, 0)
        _OperatorsB11("Operators B 11", Vector) = (0, 0, 0, 0)
        _OperatorsB12("Operators B 12", Vector) = (0, 0, 0, 0)
        _OperatorsB13("Operators B 13", Vector) = (0, 0, 0, 0)
        _OperatorsB14("Operators B 14", Vector) = (0, 0, 0, 0)
        _OperatorsB15("Operators B 15", Vector) = (0, 0, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
// Upgrade NOTE: excluded shader from DX11 because it uses wrong array syntax (type[size] name)
#pragma exclude_renderers d3d11
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _OperatorsR0, _OperatorsR1, _OperatorsR2, _OperatorsR3, _OperatorsR4, _OperatorsR5, _OperatorsR6, _OperatorsR7, _OperatorsR8, _OperatorsR9, _OperatorsR10, _OperatorsR11, _OperatorsR12, _OperatorsR13, _OperatorsR14, _OperatorsR15;
            float4 _OperatorsG0, _OperatorsG1, _OperatorsG2, _OperatorsG3, _OperatorsG4, _OperatorsG5, _OperatorsG6, _OperatorsG7, _OperatorsG8, _OperatorsG9, _OperatorsG10, _OperatorsG11, _OperatorsG12, _OperatorsG13, _OperatorsG14, _OperatorsG15;
            float4 _OperatorsB0, _OperatorsB1, _OperatorsB2, _OperatorsB3, _OperatorsB4, _OperatorsB5, _OperatorsB6, _OperatorsB7, _OperatorsB8, _OperatorsB9, _OperatorsB10, _OperatorsB11, _OperatorsB12, _OperatorsB13, _OperatorsB14, _OperatorsB15;

            int _OperatorCountR, _OperatorCountG, _OperatorCountB;

            float _ScalingFactor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

float4 GetOperator(int colorChannel, int index) {
    if (colorChannel == 0) {
        if (index == 0) {
            return _OperatorsR0;
        } else if (index == 1) {
            return _OperatorsR1;
        } else if (index == 2) {
            return _OperatorsR2;
        } else if (index == 3) {
            return _OperatorsR3;
        } else if (index == 4) {
            return _OperatorsR4;
        } else if (index == 5) {
            return _OperatorsR5;
        } else if (index == 6) {
            return _OperatorsR6;
        } else if (index == 7) {
            return _OperatorsR7;
        } else if (index == 8) {
            return _OperatorsR8;
        } else if (index == 9) {
            return _OperatorsR9;
        } else if (index == 10) {
            return _OperatorsR10;
        } else if (index == 11) {
            return _OperatorsR11;
        } else if (index == 12) {
            return _OperatorsR12;
        } else if (index == 13) {
            return _OperatorsR13;
        } else if (index == 14) {
            return _OperatorsR14;
        } else if (index == 15) {
            return _OperatorsR15;
        }
    } else if (colorChannel == 1) {
        // Repeat the above logic for G channel
        if (index == 0) {
            return _OperatorsG0;
        } else if (index == 1) {
            return _OperatorsG1;
        } else if (index == 2) {
            return _OperatorsG2;
        } else if (index == 3) {
            return _OperatorsG3;
        } else if (index == 4) {
            return _OperatorsG4;
        } else if (index == 5) {
            return _OperatorsG5;
        } else if (index == 6) {
            return _OperatorsG6;
        } else if (index == 7) {
            return _OperatorsG7;
        } else if (index == 8) {
            return _OperatorsG8;
        } else if (index == 9) {
            return _OperatorsG9;
        } else if (index == 10) {
            return _OperatorsG10;
        } else if (index == 11) {
            return _OperatorsG11;
        } else if (index == 12) {
            return _OperatorsG12;
        } else if (index == 13) {
            return _OperatorsG13;
        } else if (index == 14) {
            return _OperatorsG14;
        } else if (index == 15) {
            return _OperatorsG15;
        }
    } else if (colorChannel == 2) {
        // Repeat the above logic for B channel
        if (index == 0) {
            return _OperatorsB0;
        } else if (index == 1) {
            return _OperatorsB1;
        } else if (index == 2) {
            return _OperatorsB2;
        } else if (index == 3) {
            return _OperatorsB3;
        } else if (index == 4) {
            return _OperatorsB4;
        } else if (index == 5) {
            return _OperatorsB5;
        } else if (index == 6) {
            return _OperatorsB6;
        } else if (index == 7) {
            return _OperatorsB7;
        } else if (index == 8) {
            return _OperatorsB8;
        } else if (index == 9) {
            return _OperatorsB9;
        } else if (index == 10) {
            return _OperatorsB10;
        } else if (index == 11) {
            return _OperatorsB11;
        } else if (index == 12) {
            return _OperatorsB12;
        } else if (index == 13) {
            return _OperatorsB13;
        } else if (index == 14) {
            return _OperatorsB14;
        } else if (index == 15) {
            return _OperatorsB15;
        }
    }

    // Default case (should not happen if index is properly bounded)
    return float4(0, 0, 0, 0);
}

// from https://forum.unity.com/threads/generate-random-float-between-0-and-1-in-shader.610810/
            float random (float2 uv)
            {
                return frac(sin(dot(uv,float2(12.9898,78.233)))*43758.5453123);
            }
 
// from https://thebookofshaders.com/11/
float noise (float x, float y) {
    float2 st = float2(x, y);
    float2 i = floor(st);
    float2 f = frac(st);
    
    // Four corners in 2D of a tile
    float a = random(i);
    float b = random(i + float2(1.0, 0.0));
    float c = random(i + float2(0.0, 1.0));
    float d = random(i + float2(1.0, 1.0));

    // Smooth Interpolation

    // Cubic Hermine Curve.  Same as SmoothStep()
    float2 u = f*f*(3.0-2.0*f);
    // u = smoothstep(0.,1.,f);

    // Mix 4 corners percentages:
    return a*(1-u.x) + b*u.x +
            (c - a)* u.y * (1.0 - u.x) +
            (d - b) * u.x * u.y;
}

            float EvaluateExpression(int colorChannel, int opCount, float x, float y)
            {
                float stack[16];
                int stackIndex = 0;

                for (int i = 0; i < opCount; i++)
                {
                    float4 op = GetOperator(colorChannel, i);
                    if (op.x == 0.0) // Variable
                    {
                        stack[stackIndex++] = (op.y == 1.0) ? x : y;
                    }
                    else if (op.x == 1.0) // Constant
                    {
                        stack[stackIndex++] = op.y;
                    }
                    else // Operator
                    {
                        if(op.y < 6 || op.y > 11) {
                            float b = stack[--stackIndex];
                            float a = stack[--stackIndex];
                            if (op.y == 1.0) // +
                                stack[stackIndex++] = a + b;
                            else if (op.y == 2.0) // -
                                stack[stackIndex++] = a - b;
                            else if (op.y == 3.0) // *
                                stack[stackIndex++] = a * b;
                            else if (op.y == 4.0) // /
                                stack[stackIndex++] = a / b;
                            else if (op.y == 5.0) // pow
                                stack[stackIndex++] = pow(a, b);
                            else if (op.y == 12.0)
                                stack[stackIndex++] = a % (b*5);
                            else if (op.y == 13.0)
                                stack[stackIndex++] = (int)a | (int)b;
                            else if (op.y == 14.0)
                                stack[stackIndex++] = (int)a ^ (int)b;
                            else if (op.y == 15.0)
                                stack[stackIndex++] = (int)a & (int)b;
                            else if (op.y == 16.0)
                                stack[stackIndex++] = noise(a * 5, b * 5);
                        } else {
                            float a = stack[--stackIndex];
                            if (op.y == 6.0) // sin
                                stack[stackIndex++] = sin(a * 5);
                            else if (op.y == 7.0) // cos
                                stack[stackIndex++] = cos(a * 5);
                            else if (op.y == 8.0) // ln
                                stack[stackIndex++] = log(a * 5);
                            else if (op.y == 9.0) // sqrt
                                stack[stackIndex++] = sqrt(abs(a));
                            else if (op.y == 10.0) // exp
                                stack[stackIndex++] = exp(a);
                            else if (op.y == 11.0) // atan
                                stack[stackIndex++] = atan(a * 5);
                            }
                    }
                }

                return stack[0];
            }

            half4 frag (v2f i) : SV_Target
            {
                float x = i.uv.x * _ScalingFactor;
                float y = i.uv.y * _ScalingFactor;


                float valueR = EvaluateExpression(0, _OperatorCountR, x, y);
                float valueG = EvaluateExpression(1, _OperatorCountG, x, y);
                float valueB = EvaluateExpression(2, _OperatorCountB, x, y);
                
                return half4(valueR / _ScalingFactor, valueG / _ScalingFactor, valueB / _ScalingFactor, 1.0);
            }
            ENDCG
        }
    }
}
