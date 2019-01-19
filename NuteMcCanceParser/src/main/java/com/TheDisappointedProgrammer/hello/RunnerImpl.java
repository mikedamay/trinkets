package com.TheDisappointedProgrammer.hello;

import org.apache.commons.csv.CSVFormat;
import org.apache.commons.csv.CSVRecord;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import java.io.*;
import java.nio.charset.Charset;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.stream.Stream;
import java.util.stream.StreamSupport;

@Component
public class RunnerImpl implements Runner {
    private static final int FOOD_CODE_POS = 0;
    @Autowired
    McCanceReader mcReader;
    @Override
    public void run() {
        System.out.println("hello from runner");

        class Tuple {
            private String foodCode;
            private String nutrient;
            private String value;

            public Tuple(String foodCode, String nutrient, String value) {
                this.foodCode = foodCode;
                this.nutrient = nutrient;
                this.value = value;
            }
            public String getFoodCode() {
                return foodCode;
            }

            public String getNutrient() {
                return nutrient;
            }

            public String getValue() {
                return value;
            }

        }
        try {
            File fi = new File("C:/projects/trinkets/NuteMcCanceParser/src/main/resources/proximates.csv");
            FileInputStream fsi = new FileInputStream(fi);
            try(fsi) {
                Reader rdr = new InputStreamReader(fsi, Charset.forName("ISO8859-1"));
                var parser = mcReader.processMcCanceCSVFile(rdr);
                String[] fieldNames = parser.getFieldNames();
                List<Tuple> list = new ArrayList<Tuple>();
                for ( var ingredient : parser.getIngredientsParser().getRecords()) {
                    String foodCode = ingredient.get(FOOD_CODE_POS);
                    for (int ii = McCanceReader.NUM_GENERAL_HEADINGS; ii < ingredient.size(); ii++) {
                        list.add(new Tuple(foodCode, fieldNames[ii], ingredient.get(ii)));
                    }
                }
                System.out.println(list.get(0).foodCode);
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
