/**
 * Created by Administrator on 2017/3/30.
 */
$(document).ready(function(){
   $(".myedit").each(function(){
        $(this).mouseover(function(){
            $(this).css("z-index","2");
            $(this).children("div").addClass("myeditbti");
            $(this).children("ul").css("display","block");
        });
       $(this).mouseleave(function(){
           $(this).css("z-index","1");
           $(this).children("ul").css("display","none");
           $(this).children("div").removeClass("myeditbti");
           $(this).css("position","");
       });
   });
});