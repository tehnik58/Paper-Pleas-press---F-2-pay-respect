using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PaperPleas
{
    static class Game
    {
        //static ObjectBufer render;

        static public void InitgameObject(ref ObjectBufer render, ref Init init)
        {
            init.SetStaticObjBuffer();
            init.SetActivObjBuffer();
            init.SetMenBuffer();
            render.set(init.SetRender());

            foreach (var e in render.ActivObjBuffer)
                e.label.Click += new EventHandler(e.SwitchClick);
        }

        private static void MenLogic(ref ObjectBufer render, bool is60fps)
        {
            if (render.MenBuffer.Count > 0)
                foreach (var men in render.MenBuffer)
                {
                    if (men.label.Location.X > 1280 / 2 && !men.WithDoc)
                    {
                        men.IsCanGo = false;
                        render.ActivObjBuffer.Add(men.GetBook());
                    }

                    if (men.IsCanGo)
                        men.Move(20, 0);

                    if (men.WithDoc && men.label.Location.X > 1280 / 2)
                    {
                        men.Move(20, 0);
                        if (men.isGoHome)
                            men.MoveHome(20, 0);
                    }

                    if (men.label.Location.X > 1280)
                    {
                        render.MenBuffer.Remove(men);
                        render.MenBuffer.Add(Men.GeneratRandomMen());
                        break;
                    }

                    if (men.label.Location.X < 0)
                    {
                        render.MenBuffer.Remove(men);
                        render.MenBuffer.Add(Men.GeneratRandomMen());
                        break;
                    }
                }
        }


        private static void ActivObjectLogic(ref ObjectBufer render, Point mouse, Paper form)
        {
            foreach (var obj in render.ActivObjBuffer)
            {
                if (!obj.IsClick && Restriction.InRest(obj))
                {
                    if (CompareDocsAndSt(obj, ref render))
                        foreach (var e in render.MenBuffer)
                        {
                            if (e.id == obj.id)
                            {
                                e.IsCanGo = true;
                                e.WithDoc = true;
                                //e.label.BackColor = Color.Aqua;
                            }
                        }
                    render.ActivObjBuffer.Remove(obj);
                    break;
                }
                if (obj.IsClick)
                {
                    obj.label.Location = form.PointToClient(new Point(mouse.X - 50, mouse.Y - 25));
                }
            }
        }

        private static bool CompareDocsAndSt(Book book, ref ObjectBufer render)
        {
            foreach (var e in render.MenBuffer)
                if (e.id == book.id)
                {
                    e.IsCanGo = true;
                    return true;
                }

            return false;
        }

        static public void GameLogic(ref ObjectBufer _render, Point mouse, bool is60fps, Paper form)
        {
            MenLogic(ref _render, is60fps);
            ActivObjectLogic(ref _render, mouse, form);
        }
    }
}
