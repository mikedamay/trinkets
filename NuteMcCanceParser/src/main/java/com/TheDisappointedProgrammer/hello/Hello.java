package com.TheDisappointedProgrammer.hello;

import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.bind.annotation.RequestMapping;


@RestController
public class Hello {
    @RequestMapping("/")
    public String index() {
        return "Oy.  This is McCance";
    }
}
