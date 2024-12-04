import React from 'react';
import './App.css';
import FlipCard from './FlipCard';
import CardSetSelection from './CardSetSelection';
import QuizFlipcard from './QuizFlipcard';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

function App() {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<CardSetSelection />} />
                <Route path="/card-set/:id" element={<FlipCard />} />
                <Route path="/quizcard/:userId/:setId" element={<QuizFlipcard />} />
            </Routes>
        </Router>
    );
}

export default App;