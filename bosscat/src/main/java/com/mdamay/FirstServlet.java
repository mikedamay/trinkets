package com.mdamay;

import java.io.IOException;

/**
 * Created by IntelliJ IDEA.
 * User: Mike May
 * Date: 05/05/12
 * Time: 22:31
 * To change this template use File | Settings | File Templates.
 */
public class FirstServlet extends javax.servlet.http.HttpServlet {
    protected void doPost(javax.servlet.http.HttpServletRequest request, javax.servlet.http.HttpServletResponse response)
      throws javax.servlet.ServletException, IOException {

    }

    protected void doGet(javax.servlet.http.HttpServletRequest request, javax.servlet.http.HttpServletResponse response)
      throws javax.servlet.ServletException, IOException {
        response.getWriter().write("mike is here");
        //response.sendError(405, "Bad Stuff");
        response.flushBuffer();
    }
}
