package com.TheDisappointedProgrammer.hello;

import org.apache.commons.csv.CSVFormat;
import org.apache.commons.csv.CSVRecord;
import org.springframework.stereotype.Component;

import java.io.*;
import java.nio.charset.Charset;
import java.util.stream.Stream;
import java.util.stream.StreamSupport;

@Component
public class RunnerImpl implements Runner {
    @Override
    public void run() {
        System.out.println("hello from runner");

        try {
            File fi = new File("C:/projects/trinkets/NuteMcCanceParser/src/main/resources/proximates.csv");
            FileInputStream fsi = new FileInputStream(fi);
            try(fsi) {
                Reader rdr = new InputStreamReader(fsi, Charset.forName("ISO8859-1"));
                var records = CSVFormat.EXCEL.parse(rdr);
                var si = records.spliterator();
                var headings = StreamSupport.stream(si, false).limit(3).toArray();
                var ingredients = StreamSupport.stream(si, false).skip(3).sorted((a,b) -> a.get(1).compareTo(b.get(1))).toArray();
                System.out.println(ingredients[0]);
                for ( var r : ingredients) {
                    System.out.println(((CSVRecord)r).get(1));
                }
            }
            catch (Exception ex) {
                throw new RuntimeException(ex);
            }

        }
        catch (Exception ex2) {
            throw new RuntimeException(ex2);
        }

    }
}
