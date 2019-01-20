package com.TheDisappointedProgrammer.hello;

import org.apache.commons.csv.CSVParser;
import org.apache.commons.csv.CSVRecord;
import org.springframework.stereotype.Component;

@Component
public class IngredientNutrientTable extends McCanceTable<IngredientNutrientTable.ValuesAndFieldNames> {

    @Override
    ValuesAndFieldNames getData() {
        return new ValuesAndFieldNames(mcCanceReader.getFieldNames(), mcCanceReader.getIngredientsParser());
    }
    public class ValuesAndFieldNames {
        private String[] fieldNames;
        private CSVParser ingredientParser;

        public ValuesAndFieldNames(String[] fieldNames, CSVParser ingredientParser) {
            this.fieldNames = fieldNames;
            this.ingredientParser = ingredientParser;
        }

        public String[] getFieldNames() {
            return fieldNames;
        }

        public CSVParser getIngredientParser() {
            return ingredientParser;
        }

    }
}
