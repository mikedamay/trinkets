package com.TheDisappointedProgrammer.hello;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import java.io.*;
import java.nio.charset.Charset;
import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

@Component
public class RunnerImpl implements Runner {
    private static final int FOOD_CODE_POS = 0;
    @Autowired
    private NutrientTable nutrientTable;
    @Autowired
    private IngredientNutrientTable ingredientNutrientTable;
    @Autowired
    private IngredientTable ingredientTable;

    Logger logger = LoggerFactory.getLogger(RunnerImpl.class);

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
        logger.info("and we're off");
        FileInputStream fsi = null;
        FileInputStream fsi2 = null;
        FileInputStream fsi3 = null;
        try {
            File fi = new File("C:/projects/trinkets/NuteMcCanceParser/src/main/resources/proximates.csv");

            fsi = new FileInputStream(fi);
            Reader rdr = new InputStreamReader(fsi, Charset.forName("ISO8859-1"));
            nutrientTable.processCSVData(rdr);
            nutrientTable.getData().forEach(n -> System.out.println(String.format("%s %s", n[0], n[1])));

            fsi2 = new FileInputStream(fi);
            Reader rdr2 = new InputStreamReader(fsi2, Charset.forName("ISO8859-1"));
            ingredientNutrientTable.processCSVData(rdr2);
            List<String[]> ingredientNutrients = ingredientNutrientTable.getData().collect(Collectors.toList());

            fsi3 = new FileInputStream(fi);
            Reader rdr3 = new InputStreamReader(fsi3, Charset.forName("ISO8859-1"));
            ingredientTable.processCSVData(rdr3);
            ingredientTable.getData().forEach(rec -> System.out.println(String.format("%s, %s", rec[0], rec[1])));

        }
        catch (Exception ex2) {
            throw new RuntimeException(ex2);
        }
        finally {
            try {
                if (fsi != null)
                    fsi.close();
                if (fsi2 != null)
                    fsi2.close();
                if (fsi3 != null)
                    fsi3.close();
            }
            catch (IOException ex3) {
                logger.error("failed to close input stream");
            }

        }

    }
}
